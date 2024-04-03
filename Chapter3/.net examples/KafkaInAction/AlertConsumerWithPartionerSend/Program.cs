using Confluent.Kafka;
using AlertConsumerWithPartionerSend.Model;;

Dictionary<string, string> kaProperties = new()
{
    { "bootstrap.servers", "localhost:9092" },
    //{ "enable.auto.commit", "false" },
    { "group.id", Guid.NewGuid().ToString() } // каждый раз нужно менять это значение!!!!!!!!!!!
};

ConsumerConfig cc = new(kaProperties);
cc.AutoOffsetReset = AutoOffsetReset.Earliest;

using var consumer = new ConsumerBuilder<string, Alert>(cc)
    .SetValueDeserializer(new ValueAlertSerde())
    .Build();

// Вместо subscribte использовать assign
//consumer.Subscribe("part_test");
consumer.Assign(new TopicPartition("part_test", new Partition(0)));




bool isEnd = false;
do
{
    var consumeResult = consumer.Consume();
    Console.WriteLine(consumeResult.Key);
    Console.WriteLine($"Partition value: {consumeResult.Partition.Value}");
    var alert = consumeResult.Value;

    Console.WriteLine($"AlertId: {alert.AlertId};\r\nStageId: {alert.StageId};\r\nAlertLevel: {alert.AlertLevel};\r\nAlertMessage: {alert.AlertMessage}.\r\n");
    isEnd = consumeResult.IsPartitionEOF;
}while(!isEnd);

