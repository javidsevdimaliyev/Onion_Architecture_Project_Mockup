using AutoMapper;

namespace SolutionName.Application.Abstractions.Common.Mapper;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}
