using System;
using System.ComponentModel;
using System.Linq;

namespace Common.Utility.Extensions
{
    /// <summary>
    /// 枚举拓展类
    /// </summary>
    public static partial class EnumExtension
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
