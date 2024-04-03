using Confluent.Kafka;
using AlertProducerWithPartionerSend.Model;
using System.Text;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" }
};

Random random = new();

var topicName = "part_test";
using IProducer<string, Alert> producer = new ProducerBuilder<string, Alert>(kaProperties)
    .SetValueSerializer(new ValueAlertSerde())
    .SetPartitioner(topicName, (string topicName, int partitionCount, ReadOnlySpan<byte> keyData, bool keyIsNull) =>
    {
        var key = Encoding.UTF8.GetString(keyData);

        switch(key)
        {
            case "CRITICAL":
                return 0;
            default:
                {
                    return 1;
                }
        }
    })
    .Build();


Alert alert = new(102, "STAGE 1", "CRITICAL", "Stage 1 stopped");

Message<string, Alert> producerRecord = new();
producerRecord.Key = alert.AlertLevel;
producerRecord.Value = alert;


var result = await producer.ProduceAsync(topicName, producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp.UtcDateTime}");





