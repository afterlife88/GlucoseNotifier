using AutoMapper;
using GlucoseNotifier.Services.Models.DexcomApi.Responses;
using GlucoseNotifier.Services.ViewModels;

namespace GlucoseNotifier.Services.Configuration
{
    /// <summary>
    /// Mapping interaction API models to vm's
    /// </summary>
    public class AutomapperConfiguration
    {
        /// <summary>
        /// Configuration of automapper maps
        /// </summary>
        public static void Load()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<BGResponse, BGViewModel>()
                    .ForMember(dest => dest.SugarTime, dto => dto.MapFrom(src => src.WT.ToLocalTime()))
                    .ForMember(dest => dest.CurrentSugar, dto => dto.MapFrom(src => src.Value))
                    .ReverseMap();
            });
        }
    }
}