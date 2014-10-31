using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using System.Collection;
using System.Collections.Generic;

public class PlayerScore{
    public string Name;
    public int score;

    public PlayerScore(){
    }

    public PlayerScore(string newName, int newState){
        Name = newName;
        score = newState;
    }
}

public class SerializeManager<T> {
    public string SerializeObject(T pObject){
        string XmlizedString = null;
        MemoryStream memeoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(T));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    public object DeserizlizeObject(string pXmlizedString){
        XmlSerializer xs = new XmlSerializer(typeof(T));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        return xs.Deserialize(memoryStream);
    }

    private string UTF8ByteArrayToString(byte[] characters){
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    private byte[] StringToUTF8ByteArray(string pXmlString){
        UTF8Encoding encoding = new UTF8Encoding();
        return encoding.GetBytes(pXmlizedString);
    }
}

public class SerialiseToXML: MonoBehaviour {
    private string output = "(nothing yet)";

    void Start(){
        SerializeManager<PlayerScore> serializer =new SerializeManager<PlayerScore>();
        PlayerScore myData = new PlayerScore("matt", 200);
        output = serializer.SerializerObject(myData);
    }

    void OnGUI(){
        GUILayout.Label(output);
    }
}
