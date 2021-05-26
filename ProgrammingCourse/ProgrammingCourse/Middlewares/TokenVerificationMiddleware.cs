using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProgrammingCourse.Configurations;
using ProgrammingCourse.Models;
using ProgrammingCourse.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCourse.Middlewares
{
    public class TokenVerificationMiddleware
    {
        private RequestDelegate nextDelegate;

        public TokenVerificationMiddleware(RequestDelegate next) 
        {
            nextDelegate = next;
        }


        public async Task Invoke(HttpContext httpContext, IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<User> userMgr, RefreshTokenRepository refreshTokenRepo)
        {
            httpContext.Items["Hello"] = "Hello Here";
            Console.WriteLine("Hello MiddleWare");
            string accessToken = httpContext.Request.Cookies["accessToken"];
            string refreshToken = httpContext.Request.Cookies["refreshToken"];


            if (accessToken != null && refreshToken != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = tokenHandler.ReadJwtToken(accessToken);
                var expDate = token.ValidTo;

                if (expDate < DateTime.UtcNow)
                {
                    var nameid = token.Claims.Where(c => c.Type == "nameid").FirstOrDefault();
                    RefreshToken refresh = refreshTokenRepo.GetByUserIdAndToken(nameid.Value, refreshToken);

                    if (refresh != null)
                    {
                        if(refresh.ExpiryOn < DateTime.UtcNow)
                        {
                            await refreshTokenRepo.Delete(refresh.Id);

                            // Set Token Cookie
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Expires = DateTime.UtcNow.AddDays(-1)
                            };
                            httpContext.Response.Cookies.Append("accessToken", "", cookieOptions);
                            httpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);
                        }
                        else
                        {
                            var key = Encoding.UTF8.GetBytes(jwtTokenOptions.Value.SecretKey);
                            //var role = await userMgr.GetRolesAsync(identityUser);
                            var unique_name = token.Claims.Where(c => c.Type == "unique_name").FirstOrDefault();
                            var email = token.Claims.Where(c => c.Type == "email").FirstOrDefault();
                            var role = token.Claims.Where(c => c.Type == "role").FirstOrDefault();

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                        new Claim(ClaimTypes.NameIdentifier, nameid.Value),
                                        new Claim(ClaimTypes.Name, unique_name.Value),
                                        new Claim(ClaimTypes.Email, email.Value),
                                        new Claim(ClaimTypes.Role, role.Value)
                                   }),

                                Expires = DateTime.UtcNow.AddSeconds(jwtTokenOptions.Value.ExpiryTimeInSeconds),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                                Audience = jwtTokenOptions.Value.Audience,
                                Issuer = jwtTokenOptions.Value.Issuer
                            };

                            // Set Access Token Cookie
                            var accessTokenCookieOptions = new CookieOptions
                            {
                                HttpOnly = true
                                //Expires = DateTime.UtcNow.AddDays(7)
                            };
                            httpContext.Response.Cookies.Append("accessToken", tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)), accessTokenCookieOptions);
                        }
                    }
                }
            }

            //httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await nextDelegate.Invoke(httpContext);
        }
    }
}
