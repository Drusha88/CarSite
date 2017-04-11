using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace WebUI.Infrastructure
{
    public class MyAuthentication : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            string login = (string)filterContext.HttpContext.Session["Login"];
            string sign = (string)filterContext.HttpContext.Session["Sign"];

            if (login != null && sign != null)
            {
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
                byte[] hashCode = md5Provider.ComputeHash(Encoding.Default.GetBytes(login + ConfigurationManager.AppSettings["Key"]));
                string newSign = BitConverter.ToString(hashCode).ToLower().Replace("-", "");

                if (!(sign == newSign))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "User"}, 
                    {"action",  "Login"}
                });
            }
        }
    }
}