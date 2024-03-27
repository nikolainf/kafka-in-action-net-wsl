using System.Runtime.Serialization;

namespace AlertProducer.Model;
public class Alert : ISerializable
{
    public Alert(int alertId, string stageId, string alertLevel, string alertMessage)
    {
        _alertId = alertId;
        _stageId = stageId;
        _alertLevel = alertLevel;
        _alertMesage = alertMessage;
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
