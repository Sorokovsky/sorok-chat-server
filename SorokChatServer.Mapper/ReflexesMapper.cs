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
            if (sourceProperty is null || !sourceProperty.CanRead || !destinationProperty.CanWrite) continue;
            var value = sourceProperty.GetValue(source);
            if (value is null || !value.GetType().IsAssignableTo(destinationProperty.PropertyType))
                value = Map(value!, destinationProperty.PropertyType);

            destinationProperty.SetValue(destinationInstance, value);
        }

        return destinationInstance;
    }
}