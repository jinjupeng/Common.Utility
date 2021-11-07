using Common.Utility.JWT;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Test.JWT
{
    public class JwtHelperTest
    {
        private JwtHelper jwtHelper;

        public JwtHelperTest()
        {
            IOptions<JwtOptions> jwtOptions = Options.Create<JwtOptions>(new JwtOptions() 
            {
                Issuer = "ApiServer",
                Audience = "ApiServer",
                ExpireMinutes = "10",
                SecretKey = "a6hf18uaw3ivb754huab21n5n1"
            });
            jwtHelper = new JwtHelper(jwtOptions);
        }

        [Fact]
        public void IssueJwtTest()
        {
            var value = jwtHelper.IssueJwt(null);
            Assert.True(!string.IsNullOrWhiteSpace(value));
        }
    }
}
