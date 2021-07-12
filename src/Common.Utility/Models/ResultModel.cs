
namespace Common.Utility.Models
{
    public class MsgModel
    {
        /// <summary>
        /// 请求是否处理成功
        /// </summary>
        public bool IsOK { get; set; } = true;

        /// <summary>
        /// 请求响应状态码(200、400、500)，可自定义
        /// </summary>
        public int Code { get; set; } = 200;

        /// <summary>
        /// 请求结果描述信息
        /// </summary>
        public string Message { get; set; }
    }

    public class ResultModel<T> : MsgModel
    {
        /// <summary>
        /// 请求结果数据（通常用于查询操作）
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 请求成功的响应，不带查询数据（用于删除、修改、新增接口）
        /// </summary>
        /// <returns></returns>
        public static ResultModel<T> Success()
        {
            var msg = new ResultModel<T>
            {
                IsOK = true,
                Code = 200,
                Message = "请求响应成功!"
            };
            return msg;
        }

        /// <summary>
        /// 请求成功的响应，带有查询数据（用于数据查询接口）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ResultModel<T> Success(T obj)
        {
            var msg = new ResultModel<T>
            {
                IsOK = true,
                Code = 200,
                Message = "请求响应成功",
                Data = obj
            };
            return msg;
        }

        public static ResultModel<T> Success(string message)
        {
            var msg = new ResultModel<T>
            {
                IsOK = true,
                Code = 200,
                Message = message
            };
            return msg;
        }

        /// <summary>
        /// 请求成功的响应，带有查询数据（用于数据查询接口）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel<T> Success(T obj, string message)
        {
            var msg = new ResultModel<T>
            {
                IsOK = true,
                Code = 200,
                Message = message,
                Data = obj
            };
            return msg;
        }

        public static ResultModel<T> Fail(string message)
        {
            var msg = new ResultModel<T>
            {
                IsOK = false,
                Code = 200,
                Message = message
            };
            return msg;
        }

        public static ResultModel<T> Fail(int code, string message)
        {
            var msg = new ResultModel<T>
            {
                IsOK = false,
                Code = code,
                Message = message
            };
            return msg;
        }
    }
}
