using AutoMapper;
using StudyHub.Entities;

namespace StudyHub.BLL.Extensions;

public static class ListMapperExtension
{
    public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source, List<TDestination> destination)
    {
        var updatedList = source.Select((item, index) => mapper.Map(item, destination[index])).ToList();
        return updatedList;
    }
}
