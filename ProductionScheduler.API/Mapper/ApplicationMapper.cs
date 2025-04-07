using AutoMapper;
using ProductionScheduler.Models;
using ProductionScheduler.Models.Entities;
using ProductionScheduler.Models.Enum;
using ProductionScheduler.Models.Request;
using System.Globalization;

namespace ProductionScheduler.API.Mapper;

public class ApplicationMapper : Profile
{
    public ApplicationMapper()
    {
        CreateMap<OneTimeHolidayRequest, OneTimeHoliday>()
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => DateTime.ParseExact(src.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture)));
        CreateMap<RecurringHolidayRequest, RecurringHoliday>();
        CreateMap<WorkDayRequest, WorkDay>()
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => DateTime.ParseExact(src.StartDateTime, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.WorkdayStart, opt => opt.MapFrom(src => TimeSpan.ParseExact(src.WorkdayStart, "hh\\:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.WorkdayEnd, opt => opt.MapFrom(src => TimeSpan.ParseExact(src.WorkdayEnd, "hh\\:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => src.TimeToAdd < 0 ? CalculationType.Subtraction : CalculationType.Addition))
            .ForMember(dest => dest.WorkingDaysToAdd, opt => opt.MapFrom(src => src.TimeToAdd));
    }
}
