using business.person;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using org.kafkainaction;

string bootstrapServers = "localhost:9092";
string schemaRegistryUrl = "http://localhost:8081";
string topicName = "kinaction_schematest";
string groupName = "kinaction-schematest";

CancellationTokenSource cts = new();


    using (CachedSchemaRegistryClient schemaRegister =
        new(new SchemaRegistryConfig
        {
            Url = schemaRegistryUrl
        }))
using (var consumer =
    new ConsumerBuilder<long, User>(new ConsumerConfig
    {
        BootstrapServers = bootstrapServers,
        GroupId = groupName
    })
    .SetValueDeserializer(new AvroDeserializer<User>(schemaRegister).AsSyncOverAsync())
        .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
        .Build())
    {
    TopicPartitionOffset tps = new (new TopicPartition(topicName, 0),
 Offset.Beginning);
    consumer.Assign(tps);
    //consumer.Subscribe(topicName);
        

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(cts.Token);
                    var alert = consumeResult.Message.Value;
                    Console.WriteLine($"kinaction_info offset = {consumeResult.Offset},\r\nConsume Alert:\r\n{AlertToString(alert)}");
                    Console.WriteLine();
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }




string AlertToString(User alert)
{
    string result = $"user_id = {alert.id}\r\nuser_name={alert.name}";
    return result;
}