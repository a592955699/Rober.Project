using AutoMapper;
using Rober.Action.Model.Account;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Schedules;
using Rober.Core.Infrastructure.Mapper;
using Rober.WebApp.Framework.Models.Account;
using Rober.WebApp.Framework.Models.Areas.Account;
using Rober.WebApp.Framework.Models.Areas.Schedules;

namespace Rober.WebApp.Framework.Infrastructure.Mapper
{
    public class MapperConfiguration : Profile, IMapperProfile
    {
        public MapperConfiguration()
        {

            #region Login
            CreateMap<LoginModel, LoginRequest>()
                    .ForMember(dest => dest.SessionHead, mo => mo.Ignore())
                    .ForMember(dest => dest.Referrer, mo => mo.Ignore())
                    .ForMember(dest => dest.ClientIp, mo => mo.Ignore());

            CreateMap<LoginRequest, LoginModel>()
                .ForMember(dest => dest.VerificationCode, mo => mo.Ignore());
            #endregion

            #region User
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());

            CreateMap<UserModel, User>();
            #endregion

            #region Rule
            CreateMap<Rule, RuleModel>()
                   .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());

            CreateMap<RuleModel, Rule>();
            #endregion
            #region Schedule
            CreateMap<Schedule, ScheduleModel>()
                   .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());

            CreateMap<ScheduleModel, Schedule>();
            #endregion

        }
        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;
    }
}
