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
    GroupId = "kinaction_group_audit10fdsg223232212jlkkd32свыаf11",
    EnableAutoCommit = false,
    //EnableAutoCommit = true,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    AutoCommitIntervalMs = 10000
};

using var consumer = new ConsumerBuilder<string, string>(cc)
    .Build();

// При вызове метода Subscribe всегда срабатывает автокоммит, не смотря на установку EnableAutoCommit = false
consumer.Subscribe("kinaction_audit");

// При в вызове метода Assign, автокоммит срабатывается только при установке EnableAutoCommit = true
// И тогда важено значение параметра AutoCommitIntervalMs, автокоммит произойдет по истечение указанного времение в 
// милисекундах
consumer.Assign(new TopicPartition("kinaction_audit", new Partition(0)));



var isEnd = false;

do
{
    var record = consumer.Consume();

    Console.WriteLine(record.Message.Value);
    isEnd = record.IsPartitionEOF;

}while (!isEnd);