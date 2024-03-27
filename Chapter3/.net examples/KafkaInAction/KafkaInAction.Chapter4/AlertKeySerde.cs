using Confluent.Kafka;
using org.kafkainaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaInAction.Chapter4;
public class AlertKeySerde : ISerializer<Alert>
{
    public byte[] Serialize(Alert data, SerializationContext context)
    {
        throw new NotImplementedException();
    }
}
