using BirFikrimVar.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirFikrimVar
{
    public class UserHelper
    {
        public static String GetCurrentUserId() {
            
            return System.Web.HttpContext.Current.User.Identity.GetUserId();
        
        }
        public static string GetUserNameById(string userid)
        {
            ApplicationUser user = HttpContext
                .Current
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(userid);
            if (user != null) { 
                return user.UserName;
            
            }
            return "";
        }
    }
}