using SorokChatServer.Core.Entities;

namespace SorokChatServer.Core.Utils;

public static class RepositoryUtils
{
    private static List<string> IgnoredFields => GetIgnoredFields();

    public static TDest MergeStates<TSource, TDest>(TDest old, TSource current)
    {
        var result = Activator.CreateInstance<TDest>();
        if (old == null || current == null) throw new InvalidProgramException("Old or current state is null.");
        var oldProperties = old.GetType().GetProperties();
        var currentProperties = current.GetType().GetProperties();
        foreach (var currentProperty in currentProperties)
        {
            var oldProperty = oldProperties
                .FirstOrDefault(x => x.Name == currentProperty.Name);
            var bothHasThisProperty = oldProperty is not null;
            var isIgnored = IgnoredFields.Contains(currentProperty.Name);
            if (bothHasThisProperty)
            {
                var newValue = currentProperty.GetValue(current);
                var oldValue = oldProperty!.GetValue(old);
                var resultValue = newValue == null || isIgnored ? oldValue : newValue;
                oldProperty.SetValue(result, resultValue);
            }
        }

        return result;
    }

    private static List<string> GetIgnoredFields()
    {
        return typeof(BaseEntity)
            .GetProperties()
            .Select(x => x.Name)
            .ToList();
    }
}