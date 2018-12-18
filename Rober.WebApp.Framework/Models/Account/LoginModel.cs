namespace Rober.WebApp.Framework.Models.Account
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool Remember { get; set; }
        public string VerificationCode { get; set; }
        public string ReturnUrl { get; set; }
    }
}
