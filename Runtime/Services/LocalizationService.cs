using BetterExtensions.Runtime.Extension;
using BuildNotification.Runtime.Authorization;

namespace BuildNotification.Runtime.Services
{
    public class LocalizationService
    {
        public static string Export { get; } = "Export";
        public static string Import { get; } = "Import";
        public static string Prepare { get; } = "Prepare";
        public static string Fail { get; } = "Fail";
        public static string Success { get; } = "Success";
        public static string Ok { get; } = "OK";
        public static string Title { get; } = "Title";
        public static string Message { get; } = "Message";
        public static string Timestamp { get; } = "Timestamp";
        public static string ErrorCount { get; } = "Total Error count";
        public static string Error { get; } = "Error";
        
        public static string FirebaseAccountService { get; } = "FirebaseAdminSDKData".PrettyCamelCase();
        public static string GoogleService { get; } = nameof(ServiceInfoData).PrettyCamelCase();
    }
}