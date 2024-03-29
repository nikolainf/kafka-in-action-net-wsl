﻿using business.person;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using org.kafkainaction;

string bootstrapServers = "localhost:9092";
string schemaRegistryUrl = "http://localhost:8081";
string topicName = "kinaction_schematest";


var alertStatusArray = Enum.GetValues(typeof(AlertStatus)).Cast<AlertStatus>().ToArray();

Random random = new ();


using (CachedSchemaRegistryClient schemaRegistry =
    new(new SchemaRegistryConfig { Url = schemaRegistryUrl }))
using(var producer = 
    new ProducerBuilder<long, User>(new ProducerConfig
    {
        BootstrapServers = bootstrapServers,
        
    })
    .SetValueSerializer(new AvroSerializer<User>(schemaRegistry, new AvroSerializerConfig
    {
         BufferBytes = 100
    }))
    .Build())
{
    //Console.WriteLine("Enter any text to send Alert into Kafka or enter q to exit");
    
    //while(Console.ReadLine() != "q")
    //{
    //    int statusIndex = random.Next(0, alertStatusArray.Length);
    //    Alert alert = new()
    //    {
    //        sensor_id = 12345L,
    //        status = alertStatusArray[statusIndex],
    //        time = DateTimeOffset.Now.ToUnixTimeSeconds()
    //    };

    //    Message<long, Alert> producerRecord = new()
    //    {
    //        Key = alert.sensor_id,
    //        Value = alert
    //    };

    //    await producer.ProduceAsync(topicName, producerRecord);

    //    Console.WriteLine($"Sended Alert:\r\n{AlertToString(alert)}");
    //}

    int userId = 0;
    while(true)
    {
        Console.WriteLine("Inter your name:");
        var name = Console.ReadLine();

        if (name == null)
        {
            break;
        }

        User user = new User
        {
            id = ++userId,
            name = name,
            gender = Gender.Man

        };

        Message<long, User> record = new()
        {
            Key = user.id,
            Value = user
        };

        await producer.ProduceAsync(topicName, record);

    }
}

string AlertToString(Alert alert)
{
    string result = $"sensor_id = {alert.sensor_id}\r\ntime={alert.time}\r\nstatus={alert.status}";
    return result;
}


