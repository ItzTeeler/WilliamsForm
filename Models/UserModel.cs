namespace BackendRedo.Models;

// Think of this as the User Model
    public class UserModel
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DOB { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string Email{ get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        

        public UserModel()
        {
            
        }
    }
