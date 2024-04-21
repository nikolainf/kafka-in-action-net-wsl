using Confluent.Kafka;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" },
    { "acks", "all" },
    { "retries", "3" },
    { "max.in.flight.requests.per.connection", "1" }
};

var topicName = "kinaction_one_replica";

using var producer = new ProducerBuilder<string, string>(kaProperties)
    .Build();

Message<string, string> producerRecord = new();
producerRecord.Value = "one replica message 3";

DeliveryResult<string, string> result = await producer.ProduceAsync(topicName, producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp.UtcDateTime}");

