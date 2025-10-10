using System.Buffers.Text;
using System.Text;
using System.Text.Json.Serialization;


public class WorkViewFilterQueryResults : IWorkViewResponse
{
    [JsonPropertyName("results")]
    public List<WorkViewObject> Results { get; set; } = [];

    [JsonPropertyName("allowDirectCreate")]
    public bool AllowDirectCreate { get; set; }
}

public class WorkViewDynamicFilterQuery : WorkViewFilterQuery
{
    [JsonPropertyName("classId")]
    public string ClassId { get; set; }

    [JsonPropertyName("sourceAttribute")]
    public string SourceAttribute { get; set; }

    [JsonPropertyName("returnDistinctResults")]
    public bool ReturnDistinctResults { get; set; }

    [JsonPropertyName("sorts")]
    public List<WorkViewSort> Sorts { get; set; } = [];
}

public class WorkViewFilterQuery
{
    [JsonPropertyName("filterId")]
    public string FilterId { get; set; }

    [JsonPropertyName("constraints")]
    public List<WorkViewConstraint> Constraints { get; set; } = [];

    [JsonPropertyName("baseObject")]
    public WorkViewFilterBaseObject BaseObject { get; set; }    

    [JsonPropertyName("maxResults")]
    public string MaxResults { get; set; }

    [JsonPropertyName("truncateTextFields")]
    public bool TruncateTextFields { get; set; }
}

public class WorkViewSort
{
    [JsonPropertyName("dataAddress")]
    public string DataAddress { get; set; }

    [JsonPropertyName("sortOrder")]
    public WorkViewSortOrder SortOrder { get; set; }
}

public class WorkViewFilterBaseObject
{
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
public class WorkViewConstraint
{
    [JsonPropertyName("dataAddress")]
    public string DataAddress{ get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
    
    [JsonPropertyName("operator")]
    public WorkViewOperator Operator { get; set; }

    [JsonPropertyName("connector")]
    public WorkViewConnector Connector { get; set; }

    [JsonPropertyName("leftParenthesi")]
    public bool LeftParenthesis { get; set; }

    [JsonPropertyName("rightParenthesis")]
    public bool RightParenthesis { get; set; }

}

public class WorkViewFilterResponse
{
    [JsonPropertyName("columnAttributes")]
    public List<ColumnAttribute> ColumnAttributes { get; set; }

    [JsonPropertyName("entryAttributes")]
    public List<EntryAttribute> EntryAttributes { get; set; }

    [JsonPropertyName("fixedAttributes")]
    public List<FixedAttribute> FixedAttributes { get; set; }

    [JsonPropertyName("sortAttributes")]
    public List<SortAttribute> SortAttributes { get; set; }
}

public class ColumnAttribute
{
    [JsonPropertyName("heading")]
    public string Heading { get; set; }

    [JsonPropertyName("dataAddress")]
    public string DataAddress { get; set; }

    [JsonPropertyName("dataType")]
    public string DataType { get; set; }

    [JsonPropertyName("width")]
    public string Width { get; set; }

    [JsonPropertyName("horizontalAlignment")]
    public string HorizontalAlignment { get; set; }
}

public class EntryAttribute
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("dataAddress")]
    public string DataAddress { get; set; }

    [JsonPropertyName("dataType")]
    public WorkViewDataType DataType { get; set; }

    [JsonPropertyName("dataSetOptions")]
    public WorkViewDataSetOption DataSetOptions { get; set; }

    [JsonPropertyName("operator")]
    public string Operator { get; set; }

    [JsonPropertyName("dataSetId")]
    public string DataSetId { get; set; }
}

public class FixedAttribute
{
    [JsonPropertyName("dataAddress")]
    public string DataAddress { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("operator")]
    public WorkViewOperator Operator { get; set; }

    [JsonPropertyName("connector")]
    public WorkViewConnector Connector { get; set; }

    [JsonPropertyName("leftParenthesisCount")]
    public string LeftParenthesisCount { get; set; }

    [JsonPropertyName("rightParenthesisCount")]
    public string RightParenthesisCount { get; set; }
}

public class SortAttribute
{
    [JsonPropertyName("dataAddress")]
    public string DataAddress { get; set; }

    [JsonPropertyName("sortOrder")]
    public string SortOrder { get; set; }
}
public class WorkViewFilterList : IWorkViewResponse
{
    [JsonPropertyName("items")]
    public List<WorkViewItem> Items { get; set; }
}

public class WorkViewItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("systemName")]
    public string SystemName { get; set; }
}

public enum WorkViewDataSetOption
{
    None,
    DataSet,
    SearchAllClassResults,
    SearchFilterResultsOnly
}
public enum WorkViewDataType
{
    LargeInteger,
    Currency, 
    Float,
    Date,
    DateTime,
    Alphanumeric,
    Text,
    Relation,
    Boolean,
    Document,
    FormattedText,
    Decimal,
    EncryptedAlphanumeric
}
public enum WorkViewSortOrder
{
    ASC, DESC
}
public enum WorkViewOperator
{
    Equal,
    LessThan,
    GreaterThan,
    LessThanEqual,
    GreaterThanEqual,
    NotEqual,
    Like,
    NotLike,
    Null,
    NotNull,
    In,
    NotIn
}
public enum WorkViewConnector
{
    And,
    Or,
}