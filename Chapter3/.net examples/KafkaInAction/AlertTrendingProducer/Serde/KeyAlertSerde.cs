using AlertTrendingProducer.Model;
using Confluent.Kafka;
using System.Text;

namespace AlertTrendingProducer.Serde;
internal class KeyAlertSerde : ISerializer<Alert>
{
    public byte[] Serialize(Alert data, SerializationContext context)
    {
        var result = data.AlertId.ToString();

        return UTF8Encoding.Default.GetBytes(result);
    }
}
