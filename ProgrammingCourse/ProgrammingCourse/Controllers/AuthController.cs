using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProgrammingCourse.Configurations;
using ProgrammingCourse.Models;
using ProgrammingCourse.Models.ViewModels;
using ProgrammingCourse.Repositories;
using ProgrammingCourse.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private JwtBearerTokenSettings jwtBearerTokenSettings;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private ProgrammingCourseDbContext programmingCourseDbContext;
        private RefreshTokenRepository refreshTokenRepository;

        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ProgrammingCourseDbContext programmingCourseDbContext, RefreshTokenRepository refreshTokenRepository)
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.programmingCourseDbContext = programmingCourseDbContext;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] UserViewModel userViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(new
                {
                    Message = "Invalid Input Parameters!"
                });
            }


            bool isExisted = await EmailChecker.Check(userViewModel.Email);
            if (isExisted == false)
            {
                return BadRequest(new
                {
                    Message = "Invalid email address!"
                }); ;
            }

            //Check IsRole existed
            IdentityRole isRoleExisted = await roleManager.FindByNameAsync(userViewModel.Role);

            if (isRoleExisted == null)
            {
                return BadRequest(new
                {
                    Message = $"Role {userViewModel.Role} is invalid!"
                });
            }

            //Check IsEmail existed
            User isEmailExisted = await userManager.FindByEmailAsync(userViewModel.Email);
            if (isEmailExisted != null)
            {
                return BadRequest(new
                {
                    Message = $"Email {userViewModel.Email} is already taken."
                });
            }

            var random = new Random();
            var OTPCOde = random.Next(100000, 999999);

            var identityUser = new User() { UserName = userViewModel.UserName, Email = userViewModel.Email, AvatarUrl = "https://picsum.photos/200", IsTwoStepConfirmation = false, OTPCode = OTPCOde };

            IdentityResult result1 = await userManager.CreateAsync(identityUser, userViewModel.Password);

            if (result1.Succeeded)
            {
                IdentityResult result2 = await userManager.AddToRoleAsync(identityUser, "Student");
                if (result2.Succeeded)
                {
                    Email.SendEmailOTP(identityUser.Email, OTPCOde);
                    return Ok(
                        new
                        {
                            Result = identityUser,
                            Message = "User Registration Successful!"
                        });
                }

                return BadRequest(
                    new
                    {
                        Result = result2,
                        Message = "User Registration Unsuccessful!"
                    });
            }

            return BadRequest(
                new
                {
                    Result = result1,
                    Message = "User Registration Unsuccessful!"
                });
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] string userName, [FromForm] string email, [FromForm] string password)
        {
            User identityUser = await ValidateUser(userName, email, password);

            if (identityUser != null)
            {
                if (identityUser.IsTwoStepConfirmation == false)
                {
                    return BadRequest(
                        new
                        {
                            Message = "This account is not verified!"
                        });
                }

                var token = await GenerateTokens(identityUser);
                return Ok(new { Token = token, Message = "Login Successful" });
            }
            return BadRequest(
                new
                {
                    Message = "Login Failed"
                });
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("ResendOTP")]
        public async Task<IActionResult> ResendOTP([FromForm] string email)
        {
            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser != null)
            {
                var random = new Random();
                var OTPCOde = random.Next(100000, 999999);

                identityUser.OTPCode = OTPCOde;

                IdentityResult result = await userManager.UpdateAsync(identityUser);

                if (result.Succeeded)
                {
                    Email.SendEmailOTP(email, OTPCOde);

                    return Ok(new {
                        Message = "Resend OTP Code successfully!"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Resend OTP Code unsuccessfully!"
                    });
                }
            }
            else
            {
                return BadRequest(
                    new
                    {
                        Message = "Invalid Email!"
                    });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("VerifyTwoStepVerification")]
        public async Task<IActionResult> VerifyTwoStepVerification([FromForm] string email, [FromForm] int OTPCode)
        {
            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser != null)
            {
                if(identityUser.OTPCode == OTPCode)
                {
                    identityUser.IsTwoStepConfirmation = true;

                    IdentityResult result = await userManager.UpdateAsync(identityUser);

                    if (result.Succeeded)
                    {
                        Email.SendEmailSuccessfulVerification(email);

                        return Ok(new
                        {
                            Message = "Verify OTP Code successfully!"
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Message = "Verify OTP Code unsuccessfully!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Invalid OTP Code!"
                    });
                }
               
            }
            else
            {
                return BadRequest(
                    new
                    {
                        Message = "Invalid Email!"
                    });
            }
        }


        private async Task<User> ValidateUser(string userName, string email, string password)
        {
            var identityUser = await userManager.FindByEmailAsync(email);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            identityUser = await userManager.FindByNameAsync(userName);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }



            return null;
        }

        private async Task<ActionResult<Tuple<string, string>>> GenerateTokens(User identityUser)
        {
            // Generate access token
            string accessToken = await GenerateAccessToken(identityUser);

            // Set Access Token Cookie
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true
                //Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);


            // Generate refresh token
            string refreshToken = GenerateRefreshToken();

            // Set Refresh Token Cookie
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true
                //Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);


            // Save refresh token to database
            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                ExpiryOn = DateTime.Now.AddSeconds(jwtBearerTokenSettings.RefreshTokenExpiryInDays),
                CreatedOn = DateTime.Now,
                UserId = identityUser.Id,
                User = identityUser
            };

            programmingCourseDbContext.Add(rf);
            programmingCourseDbContext.SaveChanges();


            return new Tuple<string, string>(accessToken, refreshToken);
        }

        private string GenerateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private async Task<string> GenerateAccessToken(User identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtBearerTokenSettings.SecretKey);
            var role = await userManager.GetRolesAsync(identityUser);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                        new Claim(ClaimTypes.Email, identityUser.Email.ToString()),
                        new Claim(ClaimTypes.Role, role[0])
                   }),

                Expires = DateTime.Now.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<ActionResult<bool>> RevokeRefreshToken()
        {
            string accessToken = HttpContext.Request.Cookies["accessToken"];
            string refreshToken = HttpContext.Request.Cookies["refreshToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
            var userName = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;

            var identityUser = await userManager.FindByNameAsync(userName);

            if (identityUser == null)
            {
                return false;
            }

            var refreshTokens = refreshTokenRepository.GetByUserId(identityUser.Id);
            var selectedRefreshToken = refreshTokens.Where<RefreshToken>(c => c.Token == refreshToken).FirstOrDefault();
            if (selectedRefreshToken != null)
            {
                await refreshTokenRepository.Delete(selectedRefreshToken.Id);
            }


            // Set Refresh Token Cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext.Response.Cookies.Append("accessToken", "", cookieOptions);
            HttpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);
            return true;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<ActionResult<string>> Logout()
        {
            // Revoke Refresh Token 
            await RevokeRefreshToken();
            return Ok(new { Token = "", Message = "Logged Out" });
        }
    }
}
