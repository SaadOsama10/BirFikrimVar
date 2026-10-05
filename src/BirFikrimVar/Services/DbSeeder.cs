using BirFikrimVar.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BirFikrimVar.Services
{
    /// <summary>
    /// Replaces CreateRolesandUsers() from the MVC 5 Startup. The admin password is never hard-coded:
    /// it comes from configuration (Seed__AdminPassword) and the admin is only created when it is set.
    /// Demo content is opt-in (Seed__DemoData=true) and contains invented data only.
    /// </summary>
    public static class DbSeeder
    {
        public const string AdminRole = "Admin";

        public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<AppDbContext>();

            if (!await roleManager.RoleExistsAsync(AdminRole))
                await roleManager.CreateAsync(new IdentityRole(AdminRole));

            var adminEmail = config["Seed:AdminEmail"];
            var adminPassword = config["Seed:AdminPassword"];
            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                    var created = await userManager.CreateAsync(admin, adminPassword);
                    if (!created.Succeeded)
                        throw new InvalidOperationException("Could not create the admin user: " +
                            string.Join("; ", created.Errors.Select(e => e.Code)));
                }
                if (!await userManager.IsInRoleAsync(admin, AdminRole))
                    await userManager.AddToRoleAsync(admin, AdminRole);
            }

            if (config.GetValue<bool>("Seed:DemoData"))
                await SeedDemoDataAsync(userManager, db, config);
        }

        private static async Task SeedDemoDataAsync(UserManager<ApplicationUser> userManager, AppDbContext db, IConfiguration config)
        {
            var demoEmail = config["Seed:DemoEmail"];
            var demoPassword = config["Seed:DemoPassword"];
            ApplicationUser demo = null;
            if (!string.IsNullOrWhiteSpace(demoEmail) && !string.IsNullOrWhiteSpace(demoPassword))
            {
                demo = await userManager.FindByEmailAsync(demoEmail);
                if (demo == null)
                {
                    demo = new ApplicationUser { UserName = demoEmail, Email = demoEmail };
                    await userManager.CreateAsync(demo, demoPassword);
                }
            }

            if (await db.Posts.AnyAsync()) return;

            // Fictional authors; their passwords are random and never shown, so nobody can sign in as them.
            async Task<ApplicationUser> Author(string email)
            {
                var u = await userManager.FindByEmailAsync(email);
                if (u != null) return u;
                u = new ApplicationUser { UserName = email, Email = email };
                await userManager.CreateAsync(u, "Aa1-" + Guid.NewGuid().ToString("N"));
                return u;
            }
            var ayse = await Author("ayse@example.com");
            var omar = await Author("omar@example.com");
            var elif = await Author("elif@example.com");
            var lina = await Author("lina@example.com");

            var now = DateTime.UtcNow;
            Post P(ApplicationUser by, int daysAgo, bool? published, string title, string tags, params (string text, string image)[] plots)
            {
                var post = new Post
                {
                    Title = title, Tags = tags, Userd = by.Id, IsPublished = published,
                    PostDate = now.AddDays(-daysAgo)
                };
                var i = 1;
                foreach (var (text, image) in plots)
                    post.PostPlot.Add(new PostPlot { Text = text, ImageUrl = image, Sort = i++ });
                return post;
            }

            var posts = new[]
            {
                P(ayse, 1, true, "Study-Buddy Matcher", "study, app",
                    ("Match with classmates who are taking the same courses and prefer the same study hours. Pick your courses, pick your slots, and get a suggested partner for the week.", "/seed/idea-1.jpg"),
                    ("A first version could be a simple form and a weekly email; no complicated algorithm is needed to start.", null)),
                P(omar, 2, true, "Zero-Waste Cafeteria Week", "sustainability, campus",
                    ("One week where the cafeteria serves everything in reusable containers, with a small deposit that is refunded when the container comes back.", "/seed/idea-2.jpg"),
                    ("We can measure success by counting how much single-use packaging we avoid.", null)),
                P(elif, 3, true, "Open Lab Equipment Wiki", "engineering, community",
                    ("A shared wiki where students document how to use the lab equipment: photos, safety notes and common mistakes.", "/seed/idea-3.jpg")),
                P(lina, 4, true, "مكتبة كتب مستعملة للطلاب", "كتب، تبادل",
                    ("فكرة بسيطة: رفّ في الكلية يتبادل عليه الطلاب الكتب الدراسية المستعملة بدل شرائها كل فصل.", "/seed/idea-4.jpg")),
                P(ayse, 6, true, "Pitch Night for Student Startups", "startup, events",
                    ("A monthly evening where students pitch project ideas in three minutes to a friendly audience and collect feedback.", "/seed/idea-5.jpg"),
                    ("Volunteers from each department could act as the first mentors.", null)),
                P(omar, 8, true, "Night Shuttle Ride-Share Board", "transport, campus",
                    ("A board where students heading home late can find others on the same route and share the shuttle or a taxi.", null)),
                // Waiting in the moderation queue so the admin flow has something to review.
                P(elif, 0, null, "Peer Tutoring Marketplace", "tutoring, learning",
                    ("Students who did well in a course offer one-hour tutoring slots to others, paid in favours or coffee.", null)),
                P(lina, 0, null, "Quiet-Hours Map for Dorm Floors", "dorm, wellbeing",
                    ("A simple map showing which floors agree to quiet hours during exam weeks.", null)),
            };
            db.Posts.AddRange(posts);
            await db.SaveChangesAsync();

            db.PostLikes.AddRange(
                new PostLike { PostId = posts[0].Id, Userd = omar.Id, CreatedAt = now.AddHours(-20) },
                new PostLike { PostId = posts[0].Id, Userd = elif.Id, CreatedAt = now.AddHours(-18) },
                new PostLike { PostId = posts[1].Id, Userd = ayse.Id, CreatedAt = now.AddHours(-30) },
                new PostLike { PostId = posts[2].Id, Userd = lina.Id, CreatedAt = now.AddHours(-40) },
                new PostLike { PostId = posts[3].Id, Userd = omar.Id, CreatedAt = now.AddHours(-50) });
            db.FeedbackMessages.AddRange(
                new FeedbackMessage { PostId = posts[0].Id, UserId = omar.Id, Date = now.AddHours(-19), Text = "Great idea. Could it also suggest the library rooms that are free at that hour?" },
                new FeedbackMessage { PostId = posts[0].Id, UserId = elif.Id, Date = now.AddHours(-17), Text = "I would join the pilot for the second-year courses." },
                new FeedbackMessage { PostId = posts[1].Id, UserId = lina.Id, Date = now.AddHours(-28), Text = "We could start with just the coffee cups as a trial." });
            if (demo != null)
            {
                db.SavedPosts.Add(new SavedPost { PostId = posts[1].Id, UserId = demo.Id, Date = now.AddHours(-5) });
                db.PostLikes.Add(new PostLike { PostId = posts[2].Id, Userd = demo.Id, CreatedAt = now.AddHours(-4) });
            }
            await db.SaveChangesAsync();
        }
    }
}
