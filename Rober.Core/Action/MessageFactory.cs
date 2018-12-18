using System.Collections.Generic;
using System.Linq;
using Rober.Core.Constants;

namespace Rober.Core.Action
{
    public class MessageFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateApiRequest<T>(Microsoft.AspNetCore.Http.HttpContext httpContext) where T : SessionRequest, new()
        {
            T request = new T();
            SetSessionHead(request, httpContext);
            return request;
        }

        public static void  SetSessionHead<T>(T request, Microsoft.AspNetCore.Http.HttpContext httpContext)
            where T : SessionRequest, new()
        {
            if (request == null || httpContext == null) return;

            var sidClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == SessionConstants.SessionKeyScheme);
            var tidClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == SessionConstants.TokenKeyScheme);
            request.SessionHead = new SessionHead()
            {
                Sid = sidClaim?.Value,
                Tid = tidClaim?.Value
            };
            //IEnumerable<string> sessionKeys = httpRequest.Headers[SessionConstants.SessionKeyScheme];
            //IEnumerable<string> tokenKeys = httpRequest.Headers[SessionConstants.TokenKeyScheme];
            //var sid = sessionKeys?.FirstOrDefault();
            //var tid = tokenKeys?.FirstOrDefault();

            //if (string.IsNullOrWhiteSpace(sid) || string.IsNullOrWhiteSpace(tid))
            //{
            //    httpRequest.Cookies.TryGetValue(SessionConstants.SessionKeyScheme, out sid);
            //    httpRequest.Cookies.TryGetValue(SessionConstants.TokenKeyScheme, out tid);
            //}

            //request.SessionHead = new SessionHead()
            //{
            //    Sid = sid,
            //    Tid = tid
            //};
        }
    }
}
