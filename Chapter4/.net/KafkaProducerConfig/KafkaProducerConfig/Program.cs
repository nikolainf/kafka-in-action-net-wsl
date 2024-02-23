using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using org.kafkainaction;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

string bootstrapServers = "localhost:9092";
string schemaRegisterUrl = "http://localhost:8081";
string topicName = "acks_topic";


var alertStatusArray = Enum.GetValues(typeof(AlertStatus)).Cast<AlertStatus>().ToArray();

Random random = new();

var messageTimeoutMs = Convert.ToInt32(TimeSpan.FromSeconds(15).TotalMilliseconds);
var requestTimeoutMs = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalMilliseconds);
double lingerMs = 10;
var retryBackoffMs = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalSeconds);



ProducerConfig producerConfig = new()
{
    BootstrapServers = bootstrapServers,
    Acks = Acks.All,
    MessageSendMaxRetries = -3,
    MessageTimeoutMs = messageTimeoutMs,
    //RequestTimeoutMs = requestTimeoutMs,
    RetryBackoffMs = retryBackoffMs,
    LingerMs = lingerMs,
    //MaxInFlight = 0,

    //RetryBackoffMaxMs= retryBackoffMs,
    //ReconnectBackoffMaxMs=2,
    //ReconnectBackoffMs = 1,
    StatisticsIntervalMs = 5000,
    RequestTimeoutMs = requestTimeoutMs,
    
    
};

//producerConfig.Set("message.timeout.ms", "1500");



ShowProducerConfigInfo();




using (IProducer<long, string?> producer = new ProducerBuilder<long, string?>(producerConfig)
     .SetErrorHandler((_, e) =>
     {
         Console.WriteLine(e.ToString());
     })
     .SetLogHandler((_, e) =>
     {

     })
     .SetStatisticsHandler((_, json) =>
     {
         //json = FormatJson(json);
         //JObject jobject = JObject.Parse(json);
         ////var state = (string)jobject["brokers"]["localhost:9092/bootstrap"]["state"];
         ////var txretries = (string)jobject["brokers"]["localhost:9092/bootstrap"]["txretries"];
         //if (!end)
         //{
         //    //Console.WriteLine("State: " + state);
         //    //Console.WriteLine("Txretries" + txretries);
         //    Console.WriteLine($"Statistics: {json}");
         //    Console.WriteLine("----------------------------------------------------------------------------");
         //}
     })
    //.SetValueSerializer(new AvroSerializer<Alert>(schemaRegistry, new AvroSerializerConfig
    //{
    //    BufferBytes = 100
    //}))
    .Build())
{

    Console.Write("Enter message to produce: ");
    string message = Console.ReadLine();

    while (message != "q")
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
            Value = message
        };

        Stopwatch sw = new();

        try
        {
            sw.Start();

            var r = await producer.ProduceAsync(topicName, producerRecord);
           
            sw.Stop();
            var seconds = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"Sended message: {producerRecord.Value} for {seconds} seconds");

        }
        //catch (KafkaRetriableException ret)
        //{
        //    sw.Stop();
        //    var seconds = sw.Elapsed.TotalSeconds;
        //}
        catch (KafkaException e)
        {
            sw.Stop();
            var seconds = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"Seconds: {seconds} {e.Message}");
        }
        //finally
        //{
        //    end = true;
        //}

        Console.Write("Enter message to produce: ");
        message = Console.ReadLine();
    }

    //Console.WriteLine($"Sended Alert:\r\n{AlertToString(alert)}");
    Console.WriteLine();
    Console.WriteLine("Enter any text to send Alert into Kafka or enter q to exit");




    void ShowProducerInfo()
    {
        Console.WriteLine($"Producer name: {producer.Name}");



    }


}


void ShowProducerConfigInfo()
{
    Console.WriteLine("Producer Properties: ");

    foreach (var prop in producerConfig)
    {
        Console.WriteLine($"{prop.Key} - {prop.Value}");
    }

    Console.WriteLine();
}

string AlertToString(Alert alert)
{
    string result = $"sensor_id = {alert.sensor_id}\r\ntime={alert.time}\r\nstatus={alert.status}";
    return result;
}

static string FormatJson(string json)
{
    dynamic parsedJson = JsonConvert.DeserializeObject(json);
    return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
}
