using Rober.Action.Model.Account;
using Rober.Core.Domain.Account;
using Rober.Core.Domain.Schedules;
using Rober.Core.Extensions;
using Rober.Core.Infrastructure.Mapper;
using Rober.WebApp.Framework.Models.Account;
using Rober.WebApp.Framework.Models.Areas.Account;
using Rober.WebApp.Framework.Models.Areas.Schedules;

namespace Rober.WebApp.Framework.Infrastructure.Mapper
{
    public static class MappingExtensions
    {
        #region 泛型扩展
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
        #endregion

        #region Login
        public static LoginModel ToModel(this LoginRequest entity)
        {
            return entity.MapTo<LoginRequest, LoginModel>();
        }

        public static LoginRequest ToRequest(this LoginModel model)
        {
            return model.MapTo<LoginModel, LoginRequest>();
        }

        public static LoginRequest ToRequest(this LoginModel model, LoginRequest destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Rule
        public static RuleModel ToModel(this Rule entity)
        {
            return entity.MapTo<Rule, RuleModel>();
        }

        public static Rule ToEntity(this RuleModel model)
        {
            return model.MapTo<RuleModel, Rule>();
        }

        public static Rule ToEntity(this RuleModel model, Rule destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region User
        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        public static User ToEntity(this UserModel model)
        {
            return model.MapTo<UserModel, User>();
        }

        public static User ToEntity(this UserModel model, User destination)
        {
            return model.MapTo(destination);
        }
        #endregion

        #region Schedule
        public static ScheduleModel ToModel(this Schedule entity)
        {
            var model = entity.MapTo<Schedule, ScheduleModel>();
            model.CreatedUserName = entity.CreatedUser?.UserName;
            model.StatusName = entity.Status?.Title;
            model.SubCategoryName = entity.SubCategory?.Title;
            model.ScheduleCategoryName = entity.ScheduleCategory.ScheduleCategoryName();
            return model;
        }

        public static Schedule ToEntity(this ScheduleModel model)
        {
            return model.MapTo<ScheduleModel, Schedule>();
        }

        public static Schedule ToEntity(this ScheduleModel model, Schedule destination)
        {
            return model.MapTo(destination);
        }
        #endregion
    }
}
