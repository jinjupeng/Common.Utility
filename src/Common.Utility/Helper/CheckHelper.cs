using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utility.Helper
{
    public class CheckHelper
    {
        /// <summary>
        /// 检测对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        public static void NotNull<T>(T obj, string parameterName, string message = null)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(parameterName, message ?? $"{parameterName} is null");
        }

        /// <summary>
        /// 检测字符串是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        public static void NotNull(string obj, string parameterName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(obj))
                throw new ArgumentNullException(parameterName, message ?? $"{parameterName} is null");
        }

        /// <summary>
        /// 检测集合是否不为空且包含内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="parameterName"></param>
        public static void NotEmpty<T>(IList<T> list, string parameterName)
        {
            NotNull(list, parameterName, "collection not be null");

            if (!list.Any())
                throw new ArgumentException("collection not be empty.", parameterName);
        }
    }
}
