using Confluent.Kafka;
using System.Text;

namespace AlertConsumerWithPartionerSend.Model;
public class ValueAlertSerde : IDeserializer<Alert>
{
    public Alert Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        string value = Encoding.UTF8.GetString(data);

        string[] valueSplit = value.Split(',', StringSplitOptions.RemoveEmptyEntries);

        Alert alert = new(
            int.Parse(valueSplit[0].Replace("[", string.Empty).Replace("]", string.Empty)),
            valueSplit[1].Replace("[", string.Empty).Replace("]", string.Empty),
            valueSplit[2].Replace("[", string.Empty).Replace("]", string.Empty),
            valueSplit[3].Replace("[", string.Empty).Replace("]", string.Empty));

        return alert;
    }
}
