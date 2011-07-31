namespace EventServer.Models
{
    using System;

    public static class SystemPath
    {
        public static Func<string> BaseUri = () => "http://localhost:7508";
        public static Func<string> ApplicationPath = () => "";
    }
}
