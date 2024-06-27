using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendRedo.Models;
using Microsoft.AspNetCore.Mvc;
using WilliamsForm.Models.DTO;
using WilliamsForm.Services;

namespace WilliamsForm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;

        public UserController(UserService data)
        {
            _data = data;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO User)
        {
            return _data.Login(User);
        }

        [HttpPost]
        [Route("AddUser")]
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            return _data.AddUser(UserToAdd);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public bool UpdateUser(FormModel userToUpdate)
        {
            return _data.UpdateUser(userToUpdate);
        }

        [HttpGet]
        [Route("GetUserByEmail/{email}")]
        public UserIdDTO GetUserByEmail(string email)
        {
            return _data.GetUserIdDTOByEmail(email);
        }

        [HttpPut]
        [Route("UpdateEmail/{id}/{email}")]
        public bool UpdateUser(int id, string email)
        {
            return _data.UpdateEmail(id, email);
        }

        [HttpPut]
        [Route("ForgotPassword/{email}/{password}")]
        public bool ForgotPassword(string email, string password)
        {
            return _data.ForgotPassword(email, password);
        }

        [HttpDelete]
        [Route("DeleteUser/{userToDelete}")]
        public bool DeleteUser(string userToDelete)
        {
            return _data.DeleteUser(userToDelete);
        }
    }
}