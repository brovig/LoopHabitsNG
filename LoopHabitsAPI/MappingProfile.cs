using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace LoopHabitsAPI;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Habit, HabitDto>();
        CreateMap<Repetition, RepetitionDto>();

        CreateMap<Habit, HabitDto>().ReverseMap();
        CreateMap<Repetition, RepetitionDto>().ReverseMap();

        CreateMap<RepetitionForCreationDto, Repetition>();
        CreateMap<RepetitionForCreationDto, Repetition>().ReverseMap();

        CreateMap<RepetitionForUpdateDto, Repetition>();
    }
}
