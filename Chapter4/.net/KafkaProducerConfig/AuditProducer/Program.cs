
using Confluent.Kafka;

string bootstrapServers = "localhost:9092";
string schemaRegisterUrl = "http://localhost:8081";
string topicName = "kinaction_audit";

ProducerConfig kaProperties = new ProducerConfig();
kaProperties.Set("bootstrap.servers", "localhost:9092,localhost:9093");
kaProperties.Set("key.serializer", "org.apache.kafka.common.serialization.StringSerializer");
kaProperties.Set("value.serializer", "org.apache.kafka.common.serialization.StringSerializer");
kaProperties.Set("acks", "all");   //<2>
kaProperties.Set("retries", "3");    //<3>
kaProperties.Set("max.in.flight.requests.per.connection", "1");
