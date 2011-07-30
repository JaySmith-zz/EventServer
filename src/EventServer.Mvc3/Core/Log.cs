namespace EventServer.Core
{
    using System;
    using System.Web.Mvc;

    public interface ILoggerFactory
    {
        ILogger GetLoggerFor(Type type);
    }

    public interface ILogger
    {
        void Debug(string format, params object[] args);
        void Error(string format, params object[] args);
        void Error(Exception ex, string format, params object[] args);
    }

    public static class Log
    {
        public static ILogger For<T>(T instance)
        {
            return For(typeof(T));
        }

        public static ILogger For(Type type)
        {
            return DependencyResolver.Current.GetService<ILoggerFactory>().GetLoggerFor(type);
        }
    }
}