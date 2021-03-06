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
        private RefreshTokenRepository refreshTokenRepository;

        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, RefreshTokenRepository refreshTokenRepository)
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel userViewModel)
        {
            //Check whether email is existed
            bool isExisted = await EmailChecker.Check(userViewModel.Email);
            if (isExisted == false)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "NotExistedEmailAddress", Description = "Email address is not existed!" }
                });
            }

            //Check IsRole existed
            //IdentityRole isRoleExisted = await roleManager.FindByNameAsync(userViewModel.Role);

            //if (isRoleExisted == null)
            //{
            //    return BadRequest(new
            //    {
            //        Errors = new object[] { new { Code = "InvalidRole", Description = $"Role {userViewModel.Role} is invalid!" } }
            //    });
            //}

            //Check IsEmail existed
            User isEmailExisted = await userManager.FindByEmailAsync(userViewModel.Email);
            if (isEmailExisted != null)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidEmail", Description = $"Email {userViewModel.Email} has already taken!" }
                });
            }

            var random = new Random();
            var OTPCOde = random.Next(100000, 999999);

            var identityUser = new User() { UserName = userViewModel.UserName, Email = userViewModel.Email, AvatarUrl = "https://picsum.photos/200", IsTwoStepConfirmation = false, OTPCode = OTPCOde, IsLocked = false };

            IdentityResult result1 = await userManager.CreateAsync(identityUser, userViewModel.Password);

            if (result1.Succeeded)
            {
                IdentityResult result2 = await userManager.AddToRoleAsync(identityUser, "Student");
                if (result2.Succeeded)
                {
                    Email.SendEmailOTP(identityUser.Email, OTPCOde);
                    //return Ok(
                    //    new
                    //    {
                    //        Results = new { Code = "Success", Description = $"User registeration is successful!" } 
                    //    });

                    return Ok(
                    new
                    {
                        Results = new { RegisteredUser = identityUser }
                    });
                }

                return BadRequest(
                    new
                    {
                        Errors = result2.Errors.ToArray()[0]
                    });
            }
            else
            {
                return BadRequest(
                    new
                    {
                        Errors = result1.Errors.ToArray()[0]
                    });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserViewModel userViewModel)
        {
            User identityUser = await ValidateUser(userViewModel.UserName, userViewModel.Email, userViewModel.Password);

            if (identityUser != null)
            {
                if (identityUser.IsLocked == true)
                {
                    return Unauthorized(
                        new
                        {
                            Errors = new { Code = "LockedAccount", Description = "This account has been locked!" }
                        });
                }

                if (identityUser.IsTwoStepConfirmation == false)
                {
                    return BadRequest(
                        new
                        {
                            Errors = new
                            {
                                Email = identityUser.Email,
                                Code = "NotVerifiedAccount",
                                Description = "This account has not been verified yet!"
                            }
                        });
                }

                var token = await GenerateTokens(identityUser);
                return Ok(new
                {
                    Results = token
                });
            }
            return BadRequest(
                new
                {
                    Errors = new { Code = "InvalidAccount", Description = "This account is invalid!" }
                });
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("ResendOTP")]
        public async Task<IActionResult> ResendOTP([FromBody] ResendOTPViewModel resendOTPViewModel)
        {
            var identityUser = await userManager.FindByEmailAsync(resendOTPViewModel.Email);
            if (identityUser != null)
            {
                var random = new Random();
                var OTPCOde = random.Next(100000, 999999);

                identityUser.OTPCode = OTPCOde;

                IdentityResult result = await userManager.UpdateAsync(identityUser);

                if (result.Succeeded)
                {
                    Email.SendEmailOTP(resendOTPViewModel.Email, OTPCOde);

                    return Ok(new
                    {
                        Results = new { Code = "Success", Description = $"Resend OTPCode Successfully!" }
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "Fail", Description = "Resend OTP Code unsuccessfully!" }
                    });
                }
            }
            else
            {
                return BadRequest(
                    new
                    {
                        Errors = new { Code = "InvalidEmail", Description = "Invalid Email!" }
                    });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("VerifyTwoStepVerification")]
        public async Task<IActionResult> VerifyTwoStepVerification([FromBody] VerifyTwoStepVerificationViewModel verifyTwoStepVerificationViewModel)
        {
            var identityUser = await userManager.FindByEmailAsync(verifyTwoStepVerificationViewModel.Email);
            if (identityUser != null)
            {
                if (identityUser.OTPCode == verifyTwoStepVerificationViewModel.OTPCode)
                {
                    identityUser.IsTwoStepConfirmation = true;

                    IdentityResult result = await userManager.UpdateAsync(identityUser);

                    if (result.Succeeded)
                    {
                        Email.SendEmailSuccessfulVerification(verifyTwoStepVerificationViewModel.Email);

                        return Ok(new
                        {
                            Results = new { Code = "Success", Description = "Verify OTP Code successfully!" }
                        });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Errors = new { Code = "Fail", Description = "Verify OTP Code unsuccessfully!" }
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = new { Code = "InvalidOTPCode!", Description = "Invalid OTP Code!" }
                    });
                }

            }
            else
            {
                return BadRequest(
                    new
                    {
                        Errors = new { Code = "InvalidEmail!", Description = "Invalid Email!" }
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

        private async Task<Tuple<string, string>> GenerateTokens(User identityUser)
        {
            // Generate access token
            string accessToken = await GenerateAccessToken(identityUser);

            // Set Access Token Cookie
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
                //Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);


            // Generate refresh token
            string refreshToken = GenerateRefreshToken();

            // Set Refresh Token Cookie
            var refreshTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
                //Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);


            // Save refresh token to database
            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                ExpiryOn = DateTime.UtcNow.AddDays(jwtBearerTokenSettings.RefreshTokenExpiryInDays),
                CreatedOn = DateTime.UtcNow,
                UserId = identityUser.Id,
                User = identityUser
            };

            await refreshTokenRepository.Add(rf);


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
                        new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                        new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                        new Claim(ClaimTypes.Email, identityUser.Email.ToString()),
                        new Claim(ClaimTypes.Role, role[0])
                   }),

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<bool> RevokeRefreshToken()
        {
            string accessToken = HttpContext.Request.Cookies["accessToken"];
            string refreshToken = HttpContext.Request.Cookies["refreshToken"];

            if (accessToken == null || refreshToken == null)
            {
                return true;
            }

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
                await refreshTokenRepository.Remove(selectedRefreshToken.Id);
            }


            // Set Token Cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            HttpContext.Response.Cookies.Append("accessToken", "", cookieOptions);
            HttpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);
            return true;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Revoke Refresh Token 
            await RevokeRefreshToken();
            return Ok(new
            {
                Results = new { Code = "Success", Description = "Logged Out!" }
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            if (changePasswordViewModel.NewPassword != changePasswordViewModel.ConfirmPassword)
            {
                return BadRequest(new
                {
                    Errors = new { Code = "NotMatchNewPasswordAndConfirmPassword", Description = "New password and confirm password dont match!" }
                });
            }

            User user = await userManager.FindByIdAsync(changePasswordViewModel.UserId);

            if (user != null)
            {

                IdentityResult result = await userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);

                if (result.Succeeded)
                {

                    return Ok(new
                    {
                        Results = new { Code = "Success", Description = "Change password successfully!" }
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = result.Errors.ToArray()[0]
                    });
                }

            }
            else
            {
                return BadRequest(new
                {
                    Errors = new { Code = "InvalidUserId", Description = "UserId is invalid!" }
                });
            }
        }


        [HttpPost]
        [Route("LoginGoogle")]
        public IActionResult LoginGoogle([FromBody] string email)
        {
            return Ok(new
            {
                Results = new { Code = "Success", Description = "Login is successful!" }
            });
        }

        [HttpGet]
        [Route("IsLoggedIn")]
        public async Task<IActionResult> IsLoggedIn()
        {
            string accessToken = HttpContext.Request.Cookies["accessToken"];
            string refreshToken = HttpContext.Request.Cookies["refreshToken"];

            if (accessToken != null && refreshToken != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = tokenHandler.ReadJwtToken(accessToken);
                var expDate = token.ValidTo;

                var nameid = token.Claims.Where(c => c.Type == "nameid").FirstOrDefault();
                var unique_name = token.Claims.Where(c => c.Type == "unique_name").FirstOrDefault();
                var email = token.Claims.Where(c => c.Type == "email").FirstOrDefault();
                var role = token.Claims.Where(c => c.Type == "role").FirstOrDefault();

                User identityUser = await userManager.FindByIdAsync(nameid.Value);

                if (expDate < DateTime.UtcNow)
                {
                    RefreshToken refresh = refreshTokenRepository.GetByUserIdAndToken(nameid.Value, refreshToken);

                    if (refresh != null)
                    {
                        if (refresh.ExpiryOn < DateTime.UtcNow || identityUser.IsLocked == true)
                        {
                            await refreshTokenRepository.Remove(refresh.Id);

                            // Set Token Cookie
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTime.UtcNow.AddDays(-1)
                            };
                            HttpContext.Response.Cookies.Append("accessToken", "", cookieOptions);
                            HttpContext.Response.Cookies.Append("refreshToken", "", cookieOptions);

                            return Ok(new
                            {
                                Results = new { Code = "NotLoggedIn", Description = "Not Logged In Yet!" }
                            });
                        }
                        else
                        {
                            var key = Encoding.UTF8.GetBytes(jwtBearerTokenSettings.SecretKey);
                            //var role = await userMgr.GetRolesAsync(identityUser);

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                        new Claim(ClaimTypes.NameIdentifier, nameid.Value),
                                        new Claim(ClaimTypes.Name, unique_name.Value),
                                        new Claim(ClaimTypes.Email, email.Value),
                                        new Claim(ClaimTypes.Role, role.Value)
                                   }),

                                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                                Audience = jwtBearerTokenSettings.Audience,
                                Issuer = jwtBearerTokenSettings.Issuer
                            };

                            // Set Access Token Cookie
                            var accessTokenCookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.None
                                //Expires = DateTime.UtcNow.AddDays(7)
                            };
                            HttpContext.Response.Cookies.Append("accessToken", tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)), accessTokenCookieOptions);

                            return Ok(new
                            {
                                Results = new
                                {
                                    Info = identityUser,
                                    Role = role
                                }
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Errors = new { Code = "ExpiredToken", Description = "Token expired!" }
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        Results = identityUser,
                        Role = role
                    });
                }
            }

            return BadRequest(new
            {
                Errors = new { Code = "ExpiredToken", Description = "Token expired!" }
            });
        }
    }
}
