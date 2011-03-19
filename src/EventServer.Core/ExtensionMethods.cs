using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Xml.Serialization;
using System.Web;

public static class ExtensionMethods
{
    public static string F(this string source, object arg0)
    {
        return string.Format(source, arg0);
    }

    public static string F(this string source, object arg0, object arg1)
    {
        return string.Format(source, arg0, arg1);
    }

    public static string F(this string source, object arg0, object arg1, object arg2)
    {
        return string.Format(source, arg0, arg1, arg2);
    }

    public static string F(this string source, params object[] args)
    {
        return string.Format(source, args);
    }

    public static IEnumerable<T> NullCheck<T>(this IEnumerable<T> source)
    {
        return source ?? Enumerable.Empty<T>();
    }

    public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
    {
        var array = source.NullCheck().ToArray();
        foreach (var item in array) action(item);
        return array;
    }

    public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var array = source.NullCheck().ToArray();
        for (int i = 0; i < array.Length; i++) action(array[i], i);
        return array;
    }

    public static IEnumerable<T> TryEach<T>(this IEnumerable<T> source, Action<T> action, Action<Exception, T> onError)
    {
        return source.NullCheck().Each(item => item.Try(action, onError));
    }

    public static T Try<T>(this T item, Action<T> action, Action<Exception, T> onError)
    {
        try
        {
            action(item);
        }
        catch (Exception ex)
        {
            if (onError != null) onError(ex, item);
        }

        return item;
    }

    public static T As<T>(this object source)
    {
        return (T)source;
    }

    public static T FindById<T>(this Control control, string id) where T : Control
    {
        return control.FindControl(id).As<T>();
    }

    public static string Join(this IEnumerable<string> source, string separator)
    {
        return string.Join(separator, source.NullCheck().ToArray());
    }

    public static string Join<T>(this IEnumerable<T> source, string separator, Func<T, string> converter)
    {
        return source.NullCheck().Select(converter).Join(separator);
    }

    public static string JoinNonEmpties(this IEnumerable<string> source, string separator)
    {
        return source
            .NullCheck()
            .Where(x => x != null)
            .Where(x => !string.IsNullOrEmpty(x.Trim()))
            .Join(separator);
    }

    public static int ToInt(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0 : Convert.ToInt32(source);
    }

    public static uint ToUInt(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0U : Convert.ToUInt32(source);
    }

    public static long ToLong(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0L : Convert.ToInt64(source);
    }

    public static ulong ToULong(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0UL : Convert.ToUInt64(source);
    }

    public static decimal ToDecimal(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0M : Convert.ToDecimal(source);
    }

    public static double ToDouble(this string source)
    {
        return string.IsNullOrEmpty(source) ? 0D : Convert.ToDouble(source);
    }

    public static DateTime ToDate(this string source)
    {
        return string.IsNullOrEmpty(source) ? DateTime.MinValue : DateTime.Parse(source);
    }

    public static Uri ToUri(this string source)
    {
        return string.IsNullOrEmpty(source) ? null : new Uri(source);
    }

    public static bool Between(this DateTime date, DateTime left, DateTime right)
    {
        return left <= date && date <= right;
    }

    public static bool Between(this decimal value, decimal left, decimal right)
    {
        return left <= value && value <= right;
    }

    public static void RunAsync(this Action action)
    {
        action.BeginInvoke(action.EndInvoke, null);
    }

    public static void RunAsync<T>(this Action<T> action, T arg)
    {
        action.BeginInvoke(arg, action.EndInvoke, null);
    }

    public static void RunAsync<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
        action.BeginInvoke(arg1, arg2, action.EndInvoke, null);
    }

    public static void RunAsync<T>(this Action<T> action, T arg, TimeSpan timeToDelay)
    {
        new Timer(obj => action.BeginInvoke(arg, action.EndInvoke, null), null, (long)timeToDelay.TotalMilliseconds, Timeout.Infinite);
    }

    public static void RunAsync<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2, TimeSpan timeToDelay)
    {
        new Timer(obj => action.BeginInvoke(arg1, arg2, action.EndInvoke, null), null, (long)timeToDelay.TotalMilliseconds, Timeout.Infinite);
    }

    public static string Hash(this string source)
    {
        using (HashAlgorithm sha = new SHA256Managed())
        {
            byte[] data = Encoding.UTF8.GetBytes(source);
            sha.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(sha.Hash);
        }
    }

    public static byte[] SerializeXml<T>(this T source)
    {
        using (var memoryStream = new MemoryStream())
        {
            new XmlSerializer(typeof(T)).Serialize(memoryStream, source);
            return memoryStream.ToArray();
        }
    }

    public static T DeserializeXml<T>(this byte[] source)
    {
        using (var memoryStream = new MemoryStream(source))
            return new XmlSerializer(typeof(T)).Deserialize(memoryStream).As<T>();
    }  

    public static string MakeUrlFriendly(this string source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        source = source.Replace(" ", "-");
        while (source.Contains("--"))
            source = source.Replace("--", "-");

        source = new string(source.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());

        // diacritics
        source = source.Normalize(NormalizationForm.FormD);
        source = new string(source.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());

        return HttpUtility.UrlEncode(source).Replace("%", string.Empty);
    }

    public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
    {
        TValue value;
        return source.TryGetValue(key, out value) ? value : default(TValue);
    }

    public static TimeSpan Seconds(this int value)
    {
        return new TimeSpan(0, 0, 0, value);
    }
}