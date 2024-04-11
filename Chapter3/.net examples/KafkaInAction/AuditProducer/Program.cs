using Confluent.Kafka;
using System.Collections.Concurrent;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" },
    { "acks", "all" },
    { "retries", "3" },
    { "max.in.flight.requests.per.connection", "1" }
};


using var producer = new ProducerBuilder<string, string>(kaProperties)
    .Build();

Message<string, string> producerRecord = new();
producerRecord.Value = "audit event3032";

DeliveryResult<string, string> result = await producer.ProduceAsync("kinaction_audit", producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp.UtcDateTime}");

