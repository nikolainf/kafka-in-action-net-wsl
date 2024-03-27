// See https://aka.ms/new-console-template for more information




using Confluent.Kafka;
using KafkaInAction.Chapter4;
using org.kafkainaction;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" },
    //kaProperties.Add("key.serializer", "org.apache.kafka.common.serialization.StringSerializer");
    //kaProperties.Add("value.serializer", "org.apache.kafka.common.serialization.StringSerializer");
    { "acks", "all" },   //<2>
    { "retries", "3" },    //<3>
    { "max.in.flight.requests.per.connection", "1" },
    { "value.serializer", typeof(AlertKeySerde).ToString() }
};



using var producer = new ProducerBuilder<string, Alert>(kaProperties).SetValueSerializer(new AlertKeySerde()).Build();

Message<string, Alert> producerRecord = new();
producerRecord.Value = new Alert(1, "bA", "LEVEL HIGH", "4to-to poshlo ne tak");
var result = await producer.ProduceAsync("kinaction_audit", producerRecord);

Console.WriteLine($"kinaction_info offset = {result.Offset}, topic = {result.Topic}, timestamp = {result.Timestamp}");

Console.WriteLine("Hello, World!");
