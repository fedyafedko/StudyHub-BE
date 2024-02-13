using AutoMapper;
using StudyHub.Entities;

namespace StudyHub.BLL.Extensions;

public static class ListMapperExtension
{
    public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source, List<TDestination> destination)
    {
        for (int i = 0; i < destination.Count; i++)
        {
             mapper.Map(source[i], destination[i]);
        }
        
        return destination;
    }
}
