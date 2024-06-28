using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendRedo.Models
{
    public class FormModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address{ get; set; }
        public string? PhoneNumber{ get; set; }
        public string DOB{ get; set; }
        public string Email{ get; set; }
        public bool IsDeleted { get; set; }

         public FormModel()
         {
            
         }
    }
}