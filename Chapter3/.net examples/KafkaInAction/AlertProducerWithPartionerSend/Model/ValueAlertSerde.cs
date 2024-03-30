using Confluent.Kafka;
using System.Text;

namespace AlertProducerWithPartionerSend.Model;
public class ValueAlertSerde : ISerializer<Alert>
{
    public byte[] Serialize(Alert data, SerializationContext context)
    {
        var result = $"[{data.AlertLevel}], [{data.AlertMessage}]";

        return Encoding.UTF8.GetBytes(result);
    }
}
