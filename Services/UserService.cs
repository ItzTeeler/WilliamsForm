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

        public UpdateAccountDTO Converter(UserModel userModel)
        {
            return new UpdateAccountDTO()
            {
                ID = userModel.ID,
                Email = userModel.Email,
                IsAdmin = userModel.IsAdmin,
                IsDeleted = userModel.IsDeleted,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DOB = userModel.DOB

            };
        }

        public IEnumerable<UpdateAccountDTO> GetAllUsers()
        {
            IEnumerable<UserModel> users = _context.UserInfo.Where(user => user.IsDeleted == false);
            return users.Select(user => Converter(user)).ToList();
            
        }

        public bool DoesUserExist(string Email)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Email == Email) != null;
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
                newUser.IsAdmin = UserToAdd.IsAdmin;
                newUser.FirstName = UserToAdd.FirstName;
                newUser.LastName = UserToAdd.LastName;
                newUser.DOB = UserToAdd.DOB;
                newUser.IsDeleted = false;

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

                    Result = Ok(new { Token = tokenString, ID = foundUser.ID, IsAdmin = foundUser.IsAdmin });
                }
            }
            return Result;
        }

        public UserModel GetUserByEmail(string Email)
        {
            return _context.UserInfo.SingleOrDefault(user => user.Email == Email);
        }

        public bool UpdateUser(UpdateAccountDTO userToUpdate)
        {
            var foundUser = GetUserById(userToUpdate.ID);

            bool result = false;

            if (foundUser != null)
            {
                foundUser.ID = userToUpdate.ID;
                foundUser.Email = userToUpdate.Email;
                foundUser.IsDeleted = userToUpdate.IsDeleted;
                foundUser.FirstName = userToUpdate.FirstName;
                foundUser.LastName = userToUpdate.LastName;
                foundUser.DOB = userToUpdate.DOB;
                _context.Update<UserModel>(foundUser);

                result = _context.SaveChanges() != 0;
            }

            return result;
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

        public bool ResetPassword(string email, string password)
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
            return _context.UserInfo.SingleOrDefault(user => user.ID == id);
        }

        public bool DeleteUserByEmail(string userToDelete)
        {
            UserModel foundUser = GetUserByEmail(userToDelete);

            bool result = false;

            if (foundUser != null)
            {
                foundUser.IsDeleted = true;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public UserIdDTO GetUserIdDTOByEmail(string email)
        {
            UserIdDTO UserInfo = new UserIdDTO();

            UserModel foundUser = _context.UserInfo.SingleOrDefault(user => user.Email == email);

            UserInfo.UserId = foundUser.ID;

            UserInfo.Email = foundUser.Email;

            return UserInfo;
        }
    }
}