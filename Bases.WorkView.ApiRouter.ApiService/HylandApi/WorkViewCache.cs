using System.Reflection;
using Bases.WorkView.ApiRouter.ApiService.AssemblyGenerator;

public static class WorkViewCache
{
    public static AssemblyGenerator CustomAssembly { get; set; }
    public static WorkViewApplications? Applications { get; set; }
    public static WorkViewApplication? Application { get; set; }
    public static WorkViewClasses? Classes { get; set; }
    public static WorkViewAttributes? Attributes { get; set; }

    public static T? Find<T>(string identifyer) where T : class
    {
        if(long.TryParse(identifyer, out var id))
        {
            var item = FindById<T>(identifyer);
            return item;
        }
        else
        {
            var item = FindByName<T>(identifyer);
            return item;
        }
    }
    public static T? FindById<T>(string id) where T : class
    {
        if (typeof(T) == typeof(WorkViewApplication))
        {
            var app = WorkViewCache.Applications?.GetbyId(id);
            return app as T;
        }
        else if (typeof(T) == typeof(WorkViewClass))
        {
            var cls = WorkViewCache.Classes?.GetbyId(id);
            return cls as T;
        }
        else if (typeof(T) == typeof(WorkViewAttribute))
        {
            var attr = WorkViewCache.Attributes?.GetbyId(id);
            return attr as T;
        }
        return null;
    }
    public static T? FindByName<T>(string name) where T : class
    {
        if (typeof(T) == typeof(WorkViewApplication))
        {
            var app = WorkViewCache.Applications?.GetbyName(name);
            return app as T;
        }
        else if (typeof(T) == typeof(WorkViewClass))
        {
            var cls = WorkViewCache.Classes?.GetbyName(name);
            return cls as T;
        }
        else if (typeof(T) == typeof(WorkViewAttribute))
        {
            var attr = WorkViewCache.Attributes?.GetbyName(name);
            return attr as T;
        }
        return null;
    }

    public static Type? MapAttributeType(this WorkViewAttribute attribute)
    {
        if (Enum.TryParse<AttributeTypeName>(attribute.DataType, true, out var attr))
        {
            switch (attr)
            {
                case AttributeTypeName.Boolean:
                    return typeof(bool);
                    break;
                case AttributeTypeName.DateTime:
                case AttributeTypeName.Date:
                    return typeof(DateTime);
                case AttributeTypeName.EncryptedAlphanumeric:
                case AttributeTypeName.Text:
                case AttributeTypeName.FormattedText:
                case AttributeTypeName.Char:
                    return typeof(string);
                case AttributeTypeName.Float:
                    return typeof(float);
                case AttributeTypeName.Currency:
                case AttributeTypeName.Decimal:
                    return typeof(decimal);
                case AttributeTypeName.Document:
                case AttributeTypeName.LargeInt:
                case AttributeTypeName.Relation:
                    return typeof(long);
                default: return typeof(string);
            }
        }
        else
            throw new Exception("The attribute type that was pass is not a known attribute type.");
        
    }
}