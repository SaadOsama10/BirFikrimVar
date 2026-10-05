using System.Globalization;
using System.Resources;

namespace BirFikrimVar.Resources
{
    /// <summary>Strongly-typed accessor for PostResources.resx (en) / PostResources.ar.resx (ar).</summary>
    public static class PostResources
    {
        private static readonly ResourceManager resourceMan = new ResourceManager("BirFikrimVar.Resources.PostResources", typeof(PostResources).Assembly);

        public static CultureInfo Culture { get; set; }

        private static string Get(string name) => resourceMan.GetString(name, Culture ?? CultureInfo.CurrentUICulture) ?? name;

        public static string Create => Get("Create");
        public static string Image => Get("Image");
        public static string Sort => Get("Sort");
        public static string Post => Get("Post");
        public static string PostPlot => Get("PostPlot");
        public static string Tags => Get("Tags");
        public static string Text => Get("Text");
        public static string Title => Get("Title");
        public static string PostAddedMessage => Get("PostAddedMessage");
        public static string AdminPostsTitle => Get("AdminPostsTitle");
        public static string WriterName => Get("WriterName");
        public static string Date => Get("Date");
        public static string Details => Get("Details");
        public static string Publish => Get("Publish");
        public static string PostPublishMessage => Get("PostPublishMessage");
        public static string Reject => Get("Reject");
        public static string PostRejectMessage => Get("PostRejectMessage");
        public static string Read => Get("Read");
        public static string SaveForLater => Get("SaveForLater");
        public static string PostSavedMessage => Get("PostSavedMessage");
        public static string Saved => Get("Saved");
        public static string DeletePostMessage => Get("DeletePostMessage");
        public static string SavedPosts => Get("SavedPosts");
        public static string Comment => Get("Comment");
        public static string Send => Get("Send");
        public static string CommentMessage => Get("CommentMessage");
        public static string CommentText => Get("CommentText");
        public static string By => Get("By");
        public static string CommentSent => Get("CommentSent");
        public static string ShowComments => Get("ShowComments");
        public static string Search => Get("Search");
        public static string SearchResultLabel => Get("SearchResultLabel");
        public static string LikeRemoved => Get("LikeRemoved");
        public static string LikeAdded => Get("LikeAdded");
        public static string Liked => Get("Liked");
        public static string Like => Get("Like");
        public static string ShowLikers => Get("ShowLikers");
        public static string NoLikersYet => Get("NoLikersYet");
        public static string RegisterTitle => Get("RegisterTitle");
        public static string RegisterBtn => Get("RegisterBtn");
        public static string Email => Get("Email");
        public static string LoginTitle => Get("LoginTitle");
        public static string LocalLoginHeading => Get("LocalLoginHeading");
        public static string Password => Get("Password");
        public static string RememberMe => Get("RememberMe");
        public static string RegisterNewUser => Get("RegisterNewUser");
        public static string CreateAccountHeading => Get("CreateAccountHeading");
        public static string ConfirmPassword => Get("ConfirmPassword");
        public static string ChangePasswordTitle => Get("ChangePasswordTitle");
        public static string ChangePasswordForm => Get("ChangePasswordForm");
        public static string CurrentPassword => Get("CurrentPassword");
        public static string NewPassword => Get("NewPassword");
        public static string ConfirmNewPassword => Get("ConfirmNewPassword");
        public static string ChangePasswordButton => Get("ChangePasswordButton");
        public static string ManageTitle => Get("ManageTitle");
        public static string AccountSettingsTitle => Get("AccountSettingsTitle");
        public static string PasswordChanged => Get("PasswordChanged");
        public static string InvalidLogin => Get("InvalidLogin");
        public static string LockedOut => Get("LockedOut");
        public static string InvalidImage => Get("InvalidImage");
        public static string TooManySections => Get("TooManySections");
        public static string EmptyPost => Get("EmptyPost");
        public static string NoPostsYet => Get("NoPostsYet");
        public static string NoResults => Get("NoResults");
        public static string NoPendingPosts => Get("NoPendingPosts");
        public static string DemoAccountHint => Get("DemoAccountHint");
        public static string Back => Get("Back");
        public static string ErrorTitle => Get("ErrorTitle");
        public static string NotFoundTitle => Get("NotFoundTitle");
        public static string AdminLink => Get("AdminLink");
        public static string AddSection => Get("AddSection");
    }
}
