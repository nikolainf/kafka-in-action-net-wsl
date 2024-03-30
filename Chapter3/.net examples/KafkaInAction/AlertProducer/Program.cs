using AlertProducer.Model;
using AlertProducer.Serde;
using Confluent.Kafka;
using System.Text;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" }
};

using var producer = new ProducerBuilder<Alert, string>(kaProperties)
    .SetKeySerializer(new KeyAlertSerde())
    .SetPartitioner("kinaction_alert", (string topic, int partitionCount, ReadOnlySpan<byte> keyData, bool keyIsNull)=>
    {;
        var b1000 = Encoding.UTF8.GetString(new byte[4] { 208, 176, 208, 176 });
        var key = Encoding.UTF8.GetString(keyData);

        if(key == "CRITICAL")
        {
            return 0;
        }

        // Иначе рандом
        return 0;
    })
    .Build();

Alert alert = new(15433244, "STAGE 1", "CRITICAL", "Stage 1 stopped");

Message<Alert, string> producerRecord = new();
producerRecord.Key = alert;
producerRecord.Value = alert.AlertMessage;

var result = await producer.ProduceAsync("kinaction_alert", producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp.UtcDateTime}");



