// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.11.3
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
using System.Runtime.Serialization;

namespace org.kafkainaction
{

    public partial class Alert : ISerializable
    {

        public Alert(int alertId, string stageId, string alertLevel, string alertMesage)
        {
            _alertId = alertId;
            _stageId = stageId;
            _alertLevel = alertLevel;
            _alertMesage = alertMesage;
        }

        private int _alertId;

        private string _stageId;

        private string _alertLevel;

        private string _alertMesage;

        public int AlertId => _alertId;

        public string StageId => _stageId;

        public string AlertLevel => _alertLevel;

        public string AlertMessage => _alertMesage;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
