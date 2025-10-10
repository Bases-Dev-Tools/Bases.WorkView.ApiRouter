using System.Buffers.Text;
using System.Text;
using System.Text.Json.Serialization;


public class WorkViewCreateObject : WorkViewObjectBase
{   

    [JsonPropertyName("activateObject")]
    public bool ActivateObject { get; set; }

}

/// <summary>
/// ObjectResultModelV2
/// </summary>
public class WorkViewObject : WorkViewObjectBase
{
    [JsonPropertyName("key")]
    public string Key { get; set; }    

    public long TryGetObjectId()
    {
        try
        {
            if (!Base64.IsValid(Key))
                return -1;
            byte[] decodedBytes = Convert.FromBase64String(Key);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);
            if (!decodedString.Contains(','))
                return -1;
            string[] objectArr = decodedString.Split(',');
            if (objectArr.Length != 3)
                return -1;
            return long.Parse(objectArr[2]);
        }
        catch
        {
            return -1;
        }
    }

}

public class WorkViewObjectBase
{
    [JsonPropertyName("attributeValues")]
    public WorkViewAttributeValues AttributeValues { get; set; } = [];
}