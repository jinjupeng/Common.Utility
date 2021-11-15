using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility.JWT
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        private readonly IOptions<JwtOptions> _jwtOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtOptions"></param>
        public JwtHelper(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="customClaims"></param>
        /// <returns></returns>
        public string IssueJwt(List<Claim> customClaims)
        {
            // 这里就是声明我们的claim
            var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    // 这个就是过期时间，目前是过期60秒，可自定义，注意JWT有自己的缓冲过期时间
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(600)).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Iss,_jwtOptions.Value.Issuer),
                    new Claim(JwtRegisteredClaimNames.Aud,_jwtOptions.Value.Audience),
                    new Claim(JwtRegisteredClaimNames.Sub, _jwtOptions.Value.Sub)
                };

            if (customClaims != null)
            {
                claims.AddRange(customClaims);
            }

            // 密钥(SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                //issuer: _jwtOptions.Value.Issuer,
                //audience: _jwtOptions.Value.Audience,
                claims: claims,
                // expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        /// <summary>
        /// 获取jwt中的payLoad
        /// </summary>
        /// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public Dictionary<string, string> GetPayLoad(string encodeJwt)
        {
            if (!IsToken(encodeJwt))
            {
                return null;
            }
            var claimArr = Decode(encodeJwt);
            return claimArr.ToDictionary(x => x.Type, x => x.Value);
        }

        /// <summary>
        /// token解码
        /// </summary>
        /// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public Claim[] Decode(string encodeJwt)
        {
            if (!IsToken(encodeJwt))
            {
                return null;
            }
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(encodeJwt);
            return jwtSecurityToken?.Claims?.ToArray();
        }

        /// <summary>
        /// 刷新token值
        /// </summary>
        /// <param name="oldToken">格式：eyAAA.eyBBB.CCC</param>
        /// <returns></returns>
        public string RefreshToken(string oldToken)
        {
            if (!IsToken(oldToken))
            {
                throw new ArgumentException("Token格式不正确！");
            }
            if (!Validate(oldToken))
            {
                throw new Exception("Token已过期！");
            }
            var newToken = "";
            var oldClaims = Decode(oldToken);
            var payLoad = GetPayLoad(oldToken);
            if (ToUnixEpochDate(DateTime.UtcNow) < Convert.ToInt64(payLoad["exp"]))
            {
                // 这里就是声明我们的claim
                var claims = new List<Claim>(); // 从旧token中获取到Claim
                claims.AddRange(oldClaims.Where(t => t.Type != JwtRegisteredClaimNames.Iat));
                //重置token的发布时间为当前时间
                string nowDate = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, nowDate, ClaimValueTypes.Integer64));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken
                (
                    //issuer: _jwtOptions.Value.Issuer,
                    //audience: _jwtOptions.Value.Audience,
                    claims: claims,
                    // expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtOptions.Value.ExpireMinutes)),
                    signingCredentials: cred
                );
                newToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            return newToken;
        }

        /// <summary>
        /// 验证身份 验证签名的有效性,
        /// </summary>
        /// <param name="encodeJwt">格式：eyAAA.eyBBB.CCC</param>
        /// 例如：payLoad["aud"]?.ToString() == "roberAuddience";
        /// 例如：验证是否过期 等
        /// <returns></returns>
        public bool Validate(string encodeJwt)
        {
            if (!IsToken(encodeJwt))
            {
                return false;
            }
            var jwtArr = encodeJwt.Split('.');
            var payLoad = GetPayLoad(encodeJwt);
            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_jwtOptions.Value.SecretKey));
            var encodedSignature = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1]))));

            //首先验证签名是否正确（必须的)
            var success = string.Equals(jwtArr[2], encodedSignature);
            if (!success)
            {
                return false;
            }

            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = now <= Convert.ToInt64(payLoad["exp"]) && now >= Convert.ToInt64(payLoad["nbf"]);
            return success;
        }

        /// <summary>
        /// 判断token格式是否正确
        /// </summary>
        /// <param name="encodeJwt"></param>
        /// <returns></returns>
        public bool IsToken(string encodeJwt)
        {
            var isValidToken = new JwtSecurityTokenHandler().CanReadToken(encodeJwt);
            return isValidToken;
        }

        ///// <summary>
        ///// 获取jwt中的payload
        ///// </summary>
        ///// <param name="encodeJwt">格式：Bearer eyAAA.eyBBB.CCC</param>
        ///// <returns></returns>
        //public Dictionary<string, object> GetPayLoad(string encodeJwt)
        //{
        //    var jwtArr = encodeJwt.Split('.');
        //    var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
        //    return payLoad;
        //}

        /// <summary>
        /// datetime转时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
