# BirFikrimVar — original ASP.NET MVC 5 (.NET Framework) version

> This branch holds the **original code as written**, kept for reference. The maintained, deployable version is the ASP.NET Core 8 port on [`main`](../../tree/main).

Changes from the original, nothing else: the database credentials and the hard-coded seeded admin password were replaced with placeholders (`Web.config` → `AdminSeedEmail` / `AdminSeedPassword`; the admin account is only seeded when a password is configured), and build output, the local `.mdf` database, uploaded images, NuGet binaries and IDE files were removed. A stray duplicate `Views/Post/post/` folder was dropped.

**Requirements:** Windows, Visual Studio 2022, .NET Framework 4.7.2, SQL Server. Create a database `projectDB`, set the connection string in `BirFikrimVar/Web.config`, and restore NuGet packages. The schema lives in `BirFikrimVar.DAL/BirFikrimVarModel.edmx` (database-first).

Known issues of this version (fixed in the port): the Admin area has no `[Authorize]`, publish/reject/like are GET requests without antiforgery, Search ignores `IsPublished`, the feedback Inbox is anonymous, and uploads are not validated.
