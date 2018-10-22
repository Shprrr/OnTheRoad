
using System.Linq;

static class XmlExtension
{
    public static string ToXML(this object o)
    {
        var extraTypes = o.GetType().Assembly.GetTypes().Where(t => typeof(IItemData).IsAssignableFrom(t)).ToArray();

        var serializer = new System.Runtime.Serialization.DataContractSerializer(o.GetType(), extraTypes);
        var sw = new System.IO.StringWriter();
        var xw = new System.Xml.XmlTextWriter(sw);
        serializer.WriteObject(xw, o);
        return sw.ToString();
    }

    public static T FromXML<T>(this string str) where T : class
    {
        if (string.IsNullOrEmpty(str)) return null;

        var type = typeof(T);
        if (type.IsArray)
            type = type.GetElementType();
        var extraTypes = typeof(T).Assembly.GetTypes().Where(t => type.IsAssignableFrom(t) || typeof(Effect).IsAssignableFrom(t)).ToArray();

        var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T), extraTypes);
        var sr = new System.IO.StringReader(str);
        var xr = new System.Xml.XmlTextReader(sr);
        var o = serializer.ReadObject(xr);
        return (T)o;
    }
}
