namespace Rober.Core.Constants
{
    public class ResponseCode
    {
        #region 基本Code
        /// <summary>
        /// 系统错误
        /// </summary>
        public const int SystemError = -1;
        /// <summary>
        /// 成功
        /// </summary>
        public const int Success = 0;
        /// <summary>
        /// 业务错误
        /// </summary>
        public const int BusinessError = 1;
        /// <summary>
        /// 服务不可用
        /// </summary>
        public const int ServiceUnavaible = 2;
        /// <summary>
        /// 非法请求
        /// </summary>
        public const int InvalidRequest = 3;
        ///// <summary>
        ///// 无效的TokenId
        ///// </summary>
        //public const int InvalidToken = 4;
        /// <summary>
        /// 无效的SessionId
        /// </summary>
        public const int InvalidSession = 5;
        /// <summary>
        /// 无效的Ip
        /// </summary>
        public const int InvalidIp = 6;
        /// <summary>
        /// 非法的参数
        /// </summary>
        public const int InvalidParameters = 7;
        /// <summary>
        /// 缺少必选参数
        /// </summary>
        public const int MissingParameters = 8;
        /// <summary>
        /// 请求超时
        /// </summary>
        public const int CallTimeOut = 10;
        /// <summary>
        /// 用户调用次数超限
        /// </summary>
        public const int CallLimited = 11;
        /// <summary>
        /// 请求被禁止 
        /// </summary>
        public const int ForbiddenRequest = 12;
        /// <summary>
        /// 重复的流水号
        /// </summary>
        public const int DuplicatedSerialno = 13;
        /// <summary>
        /// 权限不足
        /// </summary>
        public const int InsufficientPrivileges = 14;
        /// <summary>
        /// 帐号在其它地方登录
        /// </summary>
        public const int ExclusiveLogin = 15;
        /// <summary>
        /// 网站维护
        /// </summary>
        public const int SiteMaintenance = 16;
        /// <summary>
        /// 会话过期
        /// </summary>
        public const int SessionTimeOut = 17;
        /// <summary>
        /// 反序列化失败
        /// </summary>
        public const int DeserializeError = 18;
        /// <summary>
        /// 无效的代理
        /// </summary>
        public const int InvalidProxy = 19;

        /// <summary>
        /// 无效的账号
        /// </summary>
        public const int InvalidAccount = 20;
        /// <summary>
        /// 无效的密码
        /// </summary>
        public const int InvalidPassowrd = 21;
        /// <summary>
        /// 无效的账号状态
        /// </summary>
        public const int InvalidAccountStatus = 22;
        /// <summary>
        /// 数据更新失败
        /// </summary>
        public const int DataChangeFail = 23;
        /// <summary>
        /// 数据未找到
        /// </summary>
        public const int DataNotFind = 24;
        #endregion
    }
}
