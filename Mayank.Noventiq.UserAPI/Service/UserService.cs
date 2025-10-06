using Mayank.Noventiq.UserAPI.Data;
using Mayank.Noventiq.UserAPI.Models;
using Mayank.Noventiq.UserAPI.Models.DTO;
using Mayank.Noventiq.UserAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Mayank.Noventiq.UserAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly AppDBContext _db;

        public UserService(IJwtTokenGenerator jwtTokenGenerator, AppDBContext db)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _db = db;
        }

        public async Task<ResponseDto> CreateUser(RegistrationRequestDto userDto)
        {
            try
            {
                bool userToReturn = await _db.Users.AnyAsync(u => u.Email == userDto.Email);
                if (!userToReturn)
                {

                    string passwordHash = HashPassword(userDto.Password);

                    User newUser = new()
                    {
                        Email = userDto.Email,
                        Name = userDto.Name,
                        RoleId = userDto.RoleId,
                        Password = passwordHash,
                        RefreshToken = _jwtTokenGenerator.GenerateRefreshToken(),
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                    };
                    await _db.Users.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    var user = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == userDto.Email);

                    return new ResponseDto { Result = new {Name = user.Name, Email = user.Email, Role = user.Role.RoleName }, IsSuccess = true, Message = "User created successfully" };
                }
                else
                {
                    return new ResponseDto { IsSuccess = false, Message = "User already created" };
                }

            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "Error Encountered " + ex.ToString() };
            }
        }

        public async Task<ResponseDto> DeleteUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return new ResponseDto { Result = null, IsSuccess = false, Message = "User not found" };
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            UserDto userdto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId
            };

            return new ResponseDto { Result = userdto, IsSuccess = true, Message = "User deleted successfully" };
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _db.Users.Select(u => new UserDto
                                    {
                                        Id = u.Id,
                                        Name = u.Name,
                                        Email = u.Email,
                                        RoleId = u.RoleId
                                    }).ToListAsync();

            return users;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.RoleId
            };
        }

        public async Task<string> GetUserRole(int id)
        {
            try
            {
                var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
                return user.Role.RoleName;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }

        public async Task<ResponseDto> UpdateUser(int id, UserNoIdDto userDto)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                    return new ResponseDto { Result = userDto, IsSuccess = false, Message = "User not found" };

                if (userDto.Name != "string")
                {
                    user.Name = userDto.Name;
                }
                if (userDto.RoleId != 0)
                {
                    user.RoleId = userDto.RoleId;
                }
                _db.Users.Update(user);
                await _db.SaveChangesAsync();


                return new ResponseDto { Result = new { Id = user.Id, Name = user.Name, Email = user.Email, RoleId = user.RoleId}, IsSuccess = true, Message = "User updated successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Result = null, IsSuccess = false, Message = "Error encountered: " + ex.Message };
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
