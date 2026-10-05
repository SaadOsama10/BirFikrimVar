using BirFikrimVar.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BirFikrimVar.Startup))]
namespace BirFikrimVar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            // هنا نقوم بإنشاء دور وحساب مدير التطبيق "Admin"
            if (!roleManager.RoleExists("Admin") && !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AdminSeedPassword"]))
            {
                IdentityRole adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                roleManager.Create(adminRole);

                // هنا نقوم بإنشاء حساب مدير التطبيق
                ApplicationUser adminUser = new ApplicationUser();
                adminUser.UserName = System.Configuration.ConfigurationManager.AppSettings["AdminSeedEmail"];
                adminUser.Email = System.Configuration.ConfigurationManager.AppSettings["AdminSeedEmail"];
                string password = System.Configuration.ConfigurationManager.AppSettings["AdminSeedPassword"];
                IdentityResult result = userManager.Create(adminUser, password);

                // نتحقق من إنشاء المستخدم بنجاح ثم نضيفه إلى دور مدير التطبيق
                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
        }
    }
}
