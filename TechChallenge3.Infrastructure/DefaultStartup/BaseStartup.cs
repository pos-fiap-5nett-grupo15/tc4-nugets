using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Text;
using TechChallenge3.Infrastructure.Crypto;
using TechChallenge3.Infrastructure.HealthCheck;
using TechChallenge3.Infrastructure.Middlewares;
using TechChallenge3.Infrastructure.Settings;
using TechChallenge3.Infrastructure.UnitOfWork;

namespace TechChallenge3.Infrastructure.DefaultStartup
{
    public class BaseStartup
    {
        public IConfiguration Configuration;

        public BaseStartup(IConfiguration configuration) =>
            this.Configuration = configuration;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<RequestCounterMiddleware>();

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");

                endpoints.MapHealthChecks("/healthcheck/app", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("app")
                });

                endpoints.MapHealthChecks("/healthcheck/db", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("db")
                });
            });

            app.UseMetricServer();
        }

        public void ConfigureService(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            var crypto = new CryptoService(this.Configuration.GetSection(nameof(CryptoSettings)).Get<CryptoSettings>());

            services.AddSingleton<ICryptoService>(crypto);

            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy("Application is running"), tags: new[] { "app" });
            services.AddHealthChecks().AddCheck("sqlserver", new SqlConnectionHealthCheck(this.Configuration, crypto), tags: new[] { "db" });

            var jwtSettings = this.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            ArgumentNullException.ThrowIfNull(jwtSettings);
            ArgumentNullException.ThrowIfNull(jwtSettings.SecretKey);

            services.Configure<JwtSettings>(this.Configuration.GetSection(nameof(JwtSettings)));

            ConfigureAuthentication(services, jwtSettings.SecretKey);
            ConfigureDatabaseService(services, this.Configuration);

            services.AddEndpointsApiExplorer();

            ConfigureSwagger(services);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Contact Manager API",
                    Version = "v1",
                    Description = "Manager contact API - FIAP Students Project"
                });

                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insert token following the pattern: \"Bearer your_authentication_token\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    Array.Empty<string>()
                }
            });
                setup.EnableAnnotations();
            });
        }

        private void ConfigureDatabaseService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITechDatabase, TechDatabase>();
        }

        private void ConfigureAuthentication(IServiceCollection services, string key)
        {
            var decryptedKey = services.BuildServiceProvider().GetService<ICryptoService>()?.Decrypt(key);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(decryptedKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}