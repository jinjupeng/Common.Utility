﻿using System.Collections.Generic;

namespace Common.Utility.Models
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> : ResultModel<List<T>>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
    }
}
