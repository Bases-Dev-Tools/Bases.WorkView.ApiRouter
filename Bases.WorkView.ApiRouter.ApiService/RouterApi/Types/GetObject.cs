using Refit;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public class PostObjectDto
{
    [JsonPropertyName("activateObject")]
    public bool? ActivateObject { get; set; }
    [JsonPropertyName("attributeValues")]
    public Dictionary<string, object> Attributes { get; set; } = [];
}

public class GetObjectDto
{
    public long objectId { get; set; }
}

public class ObjectSchema
{    
    public ObjectSchema(WorkViewClass wvclass)
    {        
        workViewClass = wvclass;
        if(workViewClass != null)
        {
            foreach(var att in workViewClass.Attributes)
            {
                var def = att.AttributeType?.Name ?? null;
                if (def != null)
                    attributes.Add(att.SystemName, def);
                else
                    Console.WriteLine($"Attribute {att.Name} does not have a type");
            }
        }
    }
    private string identifier;
    private WorkViewClass? workViewClass;
    [JsonPropertyName("attributes")]
    public Dictionary<string, dynamic> attributes { get; set;  }  = [];
}