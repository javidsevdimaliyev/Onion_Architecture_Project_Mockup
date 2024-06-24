using AutoMapper;

namespace SolutionName.Application.Abstractions.Common.Mapper;

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T)).ReverseMap();
}
