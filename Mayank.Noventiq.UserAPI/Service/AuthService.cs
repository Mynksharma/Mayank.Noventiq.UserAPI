using Azure.Core;
using Mayank.Noventiq.UserAPI.Data;
using Mayank.Noventiq.UserAPI.Models;
using Mayank.Noventiq.UserAPI.Models.DTO;
using Mayank.Noventiq.UserAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Mayank.Noventiq.UserAPI.Service
{
    public class AuthService: IAuthService  
    {
        private readonly AppDBContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDBContext db, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());

                if (user == null || user.Password != HashPassword(loginRequestDto.Password))
                {
                    return new LoginResponseDto() { User = null, AccessToken = "", RefreshToken = "" };
                }

                var roleName= _db.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.RoleName;

                string accesstoken = _jwtTokenGenerator.GenerateToken(user, roleName);
                string refreshtoken = _jwtTokenGenerator.GenerateRefreshToken();

                UserDto userDTO = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    RoleId = user.RoleId

                };

                LoginResponseDto loginResponseDto = new LoginResponseDto()
                {
                    User = userDTO,
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken

                };
                user.RefreshToken = refreshtoken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                return loginResponseDto;
            }
            catch (Exception ex)
            {
                return new LoginResponseDto() { User = null, AccessToken = "",RefreshToken="" };

            }
        }

          public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            
            try
            {
                bool userToReturn = _db.Users.Any(u => u.Email == registrationRequestDto.Email);
                if (!userToReturn)
                {

                    string passwordHash = HashPassword(registrationRequestDto.Password);

                    User newUser = new()
                    {
                        Email = registrationRequestDto.Email,
                        Name = registrationRequestDto.Name,
                        RoleId = registrationRequestDto.RoleId,
                        Password = passwordHash,
                        RefreshToken= _jwtTokenGenerator.GenerateRefreshToken(),
                        RefreshTokenExpiryTime= DateTime.Now.AddDays(7)
                    };
                    _db.Users.Add(newUser);
                    await _db.SaveChangesAsync();

                    return "Registration Successful";
                }
                else
                {
                    return "User already registered";
                }

            }
            catch (Exception ex)
            {
                return "Error: "+ex.ToString();
            }
        }

        public async Task<TokenResponseDto> Refresh(TokenRequestDto tokenRequestDto)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.RefreshToken == tokenRequestDto.RefreshToken);
                if (user == null)
                {
                    return new TokenResponseDto() { AccessToken = "", RefreshToken = "" };
                }
                var roleName = _db.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.RoleName;
                string accesstoken = _jwtTokenGenerator.GenerateToken(user, roleName);
                string refreshtoken = _jwtTokenGenerator.GenerateRefreshToken();
                user.RefreshToken = refreshtoken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

                _db.Users.Update(user);
                _db.SaveChanges();
                TokenResponseDto tokenResponseDto = new TokenResponseDto()
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken
                };

                return tokenResponseDto;
            }
            catch (Exception ex)
            {
                return new TokenResponseDto() { AccessToken = "", RefreshToken = "" };
            }

        }



        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

    }
}
