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

        CreateMap<HabitForCreationDto, Habit>();

        CreateMap<RepetitionForCreationDto, Repetition>();
        CreateMap<RepetitionForCreationDto, Repetition>().ReverseMap();

        CreateMap<Entities.SeedDataModels.Habit, HabitForCreationDto>()
            .ForMember(dest => dest.IsArchived, act => act.MapFrom(src => src.Archived))
            .ForMember(dest => dest.FrequencyDensity, act => act.MapFrom(src => src.FreqDen))
            .ForMember(dest => dest.FrequencyNumber, act => act.MapFrom(src => src.FreqNum))
            .ForMember(dest => dest.FrequencyDensity, act => act.MapFrom(src => src.FreqDen))
            .ForMember(dest => dest.ReminderTime, act => act.MapFrom(x => new DateTime(1, 1, 1, Convert.ToInt32(x.ReminderHour), Convert.ToInt32(x.ReminderMin), 0)));

        CreateMap<Entities.SeedDataModels.Repetition, RepetitionForCreationDto>()
            .ForMember(dest => dest.TimeStamp, act => act.MapFrom(srs => DateTimeOffset.FromUnixTimeMilliseconds(srs.Timestamp).UtcDateTime));
        
        CreateMap<HabitForUpdateDto, Habit>();
        CreateMap<RepetitionForUpdateDto, Repetition>();
    }
}
