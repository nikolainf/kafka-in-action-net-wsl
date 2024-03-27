using AlertProducer.Model;
using AlertProducer.Serde;
using Confluent.Kafka;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" },
    { "acks", "all"},
    {"retries", "3" },
    {"max.in.flight.requests.per.connection", "1" }
};

using var producer = new ProducerBuilder<Alert, string>(kaProperties)
    .SetKeySerializer(new KeyAlertSerde())
    .Build();

Alert alert = new(15433244, "STAGE 1", "CRITICAL", "Stage 1 stopped");

Message<Alert, string> producerRecord = new();
producerRecord.Key = alert;
producerRecord.Value = alert.AlertMessage;

var result = await producer.ProduceAsync("kinaction_alert", producerRecord);


