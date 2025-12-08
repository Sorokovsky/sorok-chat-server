using System.Reflection;

namespace SorokChatServer.Mapper;

public class ReflexesMapper : IMapper
{
    public TDest Map<TDest>(object source)
    {
        return (TDest)Map(source, typeof(TDest));
    }

    public TDest Map<TDest, TSource>(TSource source)
    {
        return (TDest)Map(source!, typeof(TDest));
    }

    private object Map(object source, Type destType)
    {
        ArgumentNullException.ThrowIfNull(source);
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        var destinationProperties = destType.GetProperties(flags);
        var destinationInstance = Activator.CreateInstance(destType);
        if (destinationInstance is null) throw new NullReferenceException();
        foreach (var destinationProperty in destinationProperties)
        {
            var sourceProperty = source.GetType().GetProperty(destinationProperty.Name, flags);
            var destinationType = destType.GetProperty(destinationProperty.Name, flags);
            if (CanMap(sourceProperty, destinationProperty))
                destinationType?.SetValue(destinationInstance, sourceProperty?.GetValue(source));
        }

        return destinationInstance;
    }

    private static bool CanMap(PropertyInfo? sourceProperty, PropertyInfo? destinationProperty)
    {
        if (destinationProperty is null || sourceProperty is null) return false;
        var canChange = sourceProperty.CanRead && destinationProperty.CanWrite;
        var isEqual = sourceProperty.PropertyType == destinationProperty.PropertyType;
        return canChange && isEqual;
    }
}