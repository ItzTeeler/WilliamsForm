using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BackendRedo.Models;
using BackendRedo.Services.Context;
using Microsoft.AspNetCore.Mvc;
using BackendRedo.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BackendRedo.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return _context.FormInfo;
        }

        public bool DoesUserExist(string Email)
        {
            return _context.FormInfo.SingleOrDefault(user => user.Email == Email) != null;
        }

        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            bool result = false;

            if (!DoesUserExist(UserToAdd.Email))
            {
                UserModel newUser = new UserModel();

                var hashPassword = HashPassword(UserToAdd.Password);

                newUser.ID = UserToAdd.ID;
                newUser.Email = UserToAdd.Email;
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;

                _context.Add(newUser);

                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public PasswordDTO HashPassword(string password)
        {
            PasswordDTO newHashPassword = new PasswordDTO();

            byte[] SaltByte = new byte[64];

            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

            provider.GetNonZeroBytes(SaltByte);

            string salt = Convert.ToBase64String(SaltByte);

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltByte, 10000);

            string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            newHashPassword.Salt = salt;
            newHashPassword.Hash = hash;

            return newHashPassword;
        }

        public bool VerifyUsersPassword(string password, string storedHash, string storedSalt)
        {
            byte[] SaltBytes = Convert.FromBase64String(storedSalt);

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltBytes, 10000);

            string newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            return newHash == storedHash;
        }

        public IActionResult Login(LoginDTO User)
        {
            IActionResult Result = Unauthorized();

            if (DoesUserExist(User.Email))
            {
                UserModel foundUser = GetUserByEmail(User.Email);
                if (VerifyUsersPassword(User.Password, foundUser.Hash, foundUser.Salt))
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    Result = Ok(new { Token = tokenString });
                }
            }
            return Result;
        }

        public UserModel GetUserByEmail(string Email)
        {
            return _context.FormInfo.SingleOrDefault(user => user.Email == Email);
        }

        public bool UpdateUser(UserModel userToUpdate)
        {
            _context.Update<UserModel>(userToUpdate);

            return _context.SaveChanges() != 0;
        }

        public bool UpdateEmail(int id, string email)
        {
            UserModel foundUser = GetUserById(id);

            bool result = false;

            if (foundUser != null)
            {
                foundUser.Email = email;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public bool ForgotPassword(string email, string password)
        {
            UserModel foundUser = GetUserByEmail(email);

            var newPassword = HashPassword(password);

            bool result = false;

            if (foundUser != null)
            {
                foundUser.Salt = newPassword.Salt;
                foundUser.Hash = newPassword.Hash;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public UserModel GetUserById(int id)
        {
            return _context.FormInfo.SingleOrDefault(user => user.ID == id);
        }

        public bool DeleteUser(string userToDelete)
        {
            UserModel foundUser = GetUserByEmail(userToDelete);

            bool result = false;

            if (foundUser != null)
            {
                _context.Remove<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public UserIdDTO GetUserIdDTOByEmail(string email)
        {
            UserIdDTO FormInfo = new UserIdDTO();

            UserModel foundUser = _context.FormInfo.SingleOrDefault(user => user.Email == email);

            FormInfo.UserId = foundUser.ID;

            FormInfo.Email = foundUser.Email;

            return FormInfo;
        }
    }
}