using Confluent.Kafka;

//Dictionary<string, string> kaProperties = new()
//{
//    {"bootstrap.servers", "localhost:9092" },
//    { "group.id", "kinaction_group_audit" },
//    { "enable.auto.commit", "false" }
//};

ConsumerConfig cc = new()
{
    BootstrapServers = "localhost:9092",
    GroupId = "KINACTION_ASSIGN_EXAMPLE1001",
    EnableAutoCommit = false,
    //EnableAutoCommit = true,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    //AutoCommitIntervalMs = 10000
    MaxPollIntervalMs = 10000,
    SessionTimeoutMs = 10000,
};

var max = cc.MaxPollIntervalMs;

using var consumer = new ConsumerBuilder<string, string>(cc)
    .Build();


// Это не так, просто группа находилась на перебалансировке: "При вызове метода Subscribe всегда срабатывает автокоммит, не смотря на установку EnableAutoCommit = false"
consumer.Subscribe("kinaction_one_replica");

// При в вызове метода Assign, автокоммит срабатывается только при установке EnableAutoCommit = true
// И тогда важено значение параметра AutoCommitIntervalMs, автокоммит произойдет по истечение указанного времение в 
// милисекундах.
// 
// Интересно что вызова Assign после Subscribe отмененяет Subscribe
//consumer.Assign(new TopicPartition("kinaction_audit", new Partition(0)));



var isEnd = false;



do
{
    var record = consumer.Consume();
    var commitResult = consumer.Commit(); 
    Console.WriteLine(record.Message.Value);
    isEnd = record.IsPartitionEOF;

}while (!isEnd);