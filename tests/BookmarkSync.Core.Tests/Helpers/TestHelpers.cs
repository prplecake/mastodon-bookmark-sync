using System.Reflection;
using System.Text;
#pragma warning disable CS8604
#pragma warning disable CS8600
#pragma warning disable CS8602
namespace BookmarkSync.Core.Tests.Helpers;

public static class EntityTests
{
    public static bool HasMethod(this object obj, string methodName)
    {
        var type = obj.GetType();
        try
        {
            return type.GetMethod(methodName) != null;
        }
        catch (AmbiguousMatchException)
        {
            return true;
        }
    }
    public static bool HasProperty(this object obj, string propertyName)
    {
        var type = obj.GetType();
        return type.GetProperty(propertyName) != null;
    }
    public static int PropertyCount(this object obj)
    {
        var type = obj.GetType();
        return type.GetProperties().Length;
    }
    public static T SetProperties<T>(T domainObject, bool recursive = false)
    {
        var props = domainObject.GetType().GetProperties();
        foreach (var prop in props)
        {
            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            try
            {
                object propObj = null;
                object data;
                switch (propType.Name.ToLower())
                {
                    case "string":
                        data = "test";
                        break;
                    case "int":
                    case "int32":
                        data = 2;
                        break;
                    case "datetime":
                        data = DateTime.Now;
                        break;
                    case "bool":
                    case "boolean":
                        data = true;
                        break;
                    case "decimal":
                        data = decimal.Parse("1.23");
                        break;
                    case "double":
                        data = double.Parse("3.21");
                        break;
                    default:
                        if (propType.IsInterface)
                        {
                            propObj = null;
                        }
                        else if (propType.IsArray)
                        {
                            var elementType = propType.GetElementType();
                            propObj = Array.CreateInstance(elementType, 1);
                        }
                        else
                        {
                            var ctr = propType.GetConstructors()[0];
                            propObj = ctr.GetParameters().Any() ? null : Activator.CreateInstance(propType);
                        }
                        data = propObj;
                        break;
                }
                if (data != null && prop.CanWrite)
                {
                    prop.SetValue(domainObject, data);
                    Assert.AreEqual(data, prop.GetValue(domainObject));
                }
                if (recursive && propObj != null)
                {
                    if (propType.IsGenericType)
                    {
                        var myListElementType = propType.GetGenericArguments().Single();
                        propObj = Activator.CreateInstance(myListElementType);
                    }
                    SetProperties(propObj, recursive);
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format(
                    "Error creating instance of {0} because: {1}",
                    propType.Name, ex.Message);
                Assert.IsNotNull(prop.GetValue(domainObject), msg);
            }
        }
        return domainObject;
    }
}
public class TestHelpers
{
    public static string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        return builder.ToString();
    }
    private static readonly Random random = new((int)DateTime.Now.Ticks);
}
