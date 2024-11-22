using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace TechChallenge3.Common.RabbitMQ
{
    public class RabbitMQManager
    {
        public static async Task Publish(
            object message,
            string hostName,
            string exchangeName,
            string routingKeyName,
            CancellationToken ct)
        {
            // Criar uma conexão com o RabbitMQ
            var factory = new ConnectionFactory() { HostName = hostName };
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
