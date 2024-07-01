using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendRedo.Models.DTO
{
    public class CreateAccountDTO
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DOB { get; set; }
        public bool IsAdmin { get; set; }
        
    }
}