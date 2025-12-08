namespace SorokChatServer.Mapper;

public interface IMapper
{
    TDest Map<TDest>(object source);
    TDest Map<TDest, TSource>(TSource source);
}