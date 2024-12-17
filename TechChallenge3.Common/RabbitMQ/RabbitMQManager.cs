using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace TechChallenge3.Common.RabbitMQ
{
    public class RabbitMQManager
    {
        public static async Task PublishAsync(
            object message,
            string hostName,
            int port,
            string userName,
            string password,
            string exchangeName,
            string routingKeyName,
            CancellationToken ct)
        {
            // Criar uma conexão com o RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                // Converter a mensagem para bytes
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                // Enviar a mensagem para a fila
                await channel.BasicPublishAsync(exchangeName, routingKeyName, body, ct);
            }
        }
    }
}
