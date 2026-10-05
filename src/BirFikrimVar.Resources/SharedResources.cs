using System.Globalization;
using System.Resources;

namespace BirFikrimVar.Resources
{
    /// <summary>Strongly-typed accessor for SharedResources.resx (en) / SharedResources.ar.resx (ar).</summary>
    public static class SharedResources
    {
        private static readonly ResourceManager resourceMan = new ResourceManager("BirFikrimVar.Resources.SharedResources", typeof(SharedResources).Assembly);

        public static CultureInfo Culture { get; set; }

        private static string Get(string name) => resourceMan.GetString(name, Culture ?? CultureInfo.CurrentUICulture) ?? name;

        public static string ApplicationDescription => Get("ApplicationDescription");
        public static string ApplicationName => Get("ApplicationName");
        public static string btnHaveIdea => Get("btnHaveIdea");
        public static string Hello => Get("Hello");
        public static string LogIn => Get("LogIn");
        public static string LogOff => Get("LogOff");
        public static string MySavedPosts => Get("MySavedPosts");
        public static string Register => Get("Register");
    }
}
