using AlertTrendingProducer.Model;
using AlertTrendingProducer.Serde;
using Confluent.Kafka;

Dictionary<string, string> kaProperties = new()
{
     { "bootstrap.servers", "localhost:9092" }
};

using IProducer<Alert, string> producer = new ProducerBuilder<Alert, string>(kaProperties)
    .SetKeySerializer(new KeyAlertSerde())
    .Build();

Alert alert = new(0, "Stage 0", "CRITICAL", "Stage 0 stopped");
Message<Alert, string> producerRecord = new();
producerRecord.Key = alert;
producerRecord.Value = alert.AlertMessage;

var result = await producer.ProduceAsync("kinaction_alerttrend", producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp.UtcDateTime}");


//try (Producer < Alert, String > producer = new KafkaProducer<>(kaProperties)) {
//    Alert alert = new Alert(0, "Stage 0", "CRITICAL", "Stage 0 stopped");
//    ProducerRecord<Alert, String> producerRecord =
//        new ProducerRecord<>("kinaction_alerttrend", alert, alert.getAlertMessage());    //<2>

//    RecordMetadata result = producer.send(producerRecord).get();
//    log.info("kinaction_info offset = {}, topic = {}, timestamp = {}",
//             result.offset(), result.topic(), result.timestamp());
//}