using System;

namespace EventServer.Core
{
   public static class Log
   {
      public static ILogger For<T>(T instance)
      {
         return For(typeof(T));
      }

      public static ILogger For(Type type)
      {
         return Ioc.Resolve<ILoggerFactory>().GetLoggerFor(type);
      }
   }

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
}