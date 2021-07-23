using System;
using System.Globalization;

namespace Common.Utility.Extensions
{
    public static partial class DateTimeExtention
    {
        /// <summary>
        /// 时间戳起始日期
        /// </summary>
        public static DateTime TimestampStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 转换为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="milliseconds">是否使用毫秒</param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime, bool milliseconds = false)
        {
            var timestamp = dateTime.ToUniversalTime() - TimestampStart;
            return (long)(milliseconds ? timestamp.TotalMilliseconds : timestamp.TotalSeconds);
        }

        /// <summary>
        /// 获取周几
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetWeekName(this DateTime datetime)
        {
            var day = (int)datetime.DayOfWeek;
            var week = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return week[day];
        }

        ///   <summary> 
        ///  获取某一日期是该年中的第几周
        ///   </summary> 
        ///   <param name="dateTime"> 日期 </param> 
        ///   <returns> 该日期在该年中的周数 </returns> 
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取Js格式的timestamp
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <returns>单位：毫秒</returns>
        public static long ToJsTimestamp(this DateTime dateTime)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            long result = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位
            return result;
        }

        /// <summary>
        /// js时间戳转日期格式
        /// </summary>
        /// <param name="jsTimeStamp">单位：毫秒</param>
        /// <returns></returns>
        public static DateTime ToJsDateTime(this long jsTimeStamp)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = jsTimeStamp * 10000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 获取js中的getTime()
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static Int64 JsGetTime(this DateTime dt)
        {
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = dt.ToUniversalTime() - st;
            long retval = (long)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        /// <summary>
        /// 返回默认时间1970-01-01
        /// </summary>
        /// <param name="dt">时间日期</param>
        /// <returns></returns>
        public static DateTime Default(this DateTime dt)
        {
            return DateTime.Parse("1970-01-01");
        }

        /// <summary>
        /// 转为本地时间
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this DateTime time)
        {
            return TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local);
        }

        /// <summary>
        /// 转为转换为Unix时间戳格式(精确到秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>单位：秒</returns>
        public static int ToUnixTimeStamp(this DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// unix时间戳转日期格式
        /// </summary>
        /// <param name="unixTimeStamp">单位：秒</param>
        /// <returns></returns>
        public static DateTime ToUnixDateTime(this long unixTimeStamp)
        {
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = unixTimeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
