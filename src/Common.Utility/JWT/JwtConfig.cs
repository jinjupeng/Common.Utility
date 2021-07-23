using Common.Utility.Extensions;
using System;

namespace Common.Utility.JWT
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 证书颁发者
        /// </summary>
        public string Iss { get; set; } = "ApiServer";

        /// <summary>
        /// 接收jwt的一方
        /// </summary>
        public string Aud { get; set; }

        /// <summary>
        /// 加密字符串
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// jwt的过期时间（时间戳），这个过期时间必须要大于签发时间
        /// </summary>
        public long Exp { get; set; } = DateTime.Now.AddMinutes(60).ToTimestamp();

        /// <summary>
        /// jwt所面向的用户
        /// </summary>
        public string Sub { get; set; } = "";

        /// <summary>
        /// jwt的签发时间（时间戳），默认当前时间
        /// </summary>
        public long Iat { get; set; } = DateTime.Now.ToTimestamp();

        /// <summary>
        /// jti: jwt的唯一身份标识，主要用来作为一次性token,从而回避重放攻击。
        /// </summary>
        public string Jti { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 定义在什么时间之前（时间戳），该jwt都是不可用的
        /// </summary>
        public long Nbf { get; set; } = DateTime.Now.ToTimestamp();
    }
}
