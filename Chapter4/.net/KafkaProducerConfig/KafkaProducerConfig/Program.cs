using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using org.kafkainaction;
using static System.Net.Mime.MediaTypeNames;

string bootstrapServers = "localhost:9092";
string schemaRegisterUrl = "http://localhost:8081";
string topicName = "kinaction_producer";


var alertStatusArray = Enum.GetValues(typeof(AlertStatus)).Cast<AlertStatus>().ToArray();

Random random = new();

//using (CachedSchemaRegistryClient schemaRegistry =
//    new(new SchemaRegistryConfig { Url = schemaRegisterUrl }))
using (var producer
    = new ProducerBuilder<long, string?>(new ProducerConfig
    {
        BootstrapServers = bootstrapServers,
        Acks = Acks.All
    })
    //.SetValueSerializer(new AvroSerializer<Alert>(schemaRegistry, new AvroSerializerConfig
    //{
    //    BufferBytes = 100
    //}))
    .Build())
{
    Console.WriteLine("Enter any text to send Alert into Kafka or enter q to exit");

    string? text = string.Empty;
    while ((text = Console.ReadLine()) != "q")
    {
        int statusIndex = random.Next(0, alertStatusArray.Length);
        Alert alert = new()
        {
            sensor_id = 12345L,
            status = alertStatusArray[statusIndex],
            time = DateTimeOffset.Now.ToUnixTimeSeconds()
        };

        Message<long, string?> producerRecord = new()
        {
            Key = alert.sensor_id,
            Value = text
        };

        await producer.ProduceAsync(topicName, producerRecord);

        Console.WriteLine($"Sended text: {text}");
        //Console.WriteLine($"Sended Alert:\r\n{AlertToString(alert)}");
        Console.WriteLine();
        Console.WriteLine("Enter any text to send Alert into Kafka or enter q to exit");

    }
}


string AlertToString(Alert alert)
{
    string result = $"sensor_id = {alert.sensor_id}\r\ntime={alert.time}\r\nstatus={alert.status}";
    return result;
}
