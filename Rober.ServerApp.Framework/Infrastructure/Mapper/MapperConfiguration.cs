using AutoMapper;
using Rober.Core.Infrastructure.Mapper;

namespace Rober.ServerApp.Framework.Infrastructure.Mapper
{
    public class MapperConfiguration : Profile, IMapperProfile
    {
        public MapperConfiguration()
        {
            ////countries
            //CreateMap<CountryModel, Country>()
            //    .ForMember(dest => dest.StateProvinces, mo => mo.Ignore())
            //    .ForMember(dest => dest.RestrictedShippingMethods, mo => mo.Ignore())
            //    .ForMember(dest => dest.LimitedToStores, mo => mo.Ignore());
            //CreateMap<Country, CountryModel>()
            //    .ForMember(dest => dest.NumberOfStates,
            //        mo => mo.MapFrom(src => src.StateProvinces != null ? src.StateProvinces.Count : 0))
            //    .ForMember(dest => dest.Locales, mo => mo.Ignore())
            //    .ForMember(dest => dest.AvailableStores, mo => mo.Ignore())
            //    .ForMember(dest => dest.SelectedStoreIds, mo => mo.Ignore())
            //    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
        }
        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;
    }
}
