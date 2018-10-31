using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

static class XmlExtension
{
    public static string ToXML(this object o)
    {
        var extraTypes = o.GetType().Assembly.GetTypes().Where(t => typeof(IItemData).IsAssignableFrom(t)).ToArray();

        var serializer = new DataContractSerializer(o.GetType(), extraTypes);
        var sw = new StringWriter();
        var xw = new XmlTextWriter(sw);
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

        var serializer = new DataContractSerializer(typeof(T), extraTypes);
        var sr = new StringReader(str);
        var xr = new XmlTextReader(sr);
        var o = serializer.ReadObject(xr);
        return (T)o;
    }
}
