namespace SorokChatServer.Mapper;

public record MapperConfiguration(IEnumerable<TypeMappingConfiguration> TypeMappings);

public record TypeMappingConfiguration(
    Type SourceType,
    Type DestType,
    IEnumerable<MemberMappingConfiguration> MemberMappingConfigurations);

public record MemberMappingConfiguration(string MemberName, IMappingAction MappingAction);

public interface IMappingAction
{
}