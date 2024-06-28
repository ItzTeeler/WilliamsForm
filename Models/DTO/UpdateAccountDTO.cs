using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendRedo.Models.DTO
{
    public class UpdateAccountDTO
    {
        public int ID { get; set; }
        public string? Email{ get; set; }
        public bool? isAdmin { get; set; }
        public bool? isDeleted { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? DOB { get; set; }
    }
}