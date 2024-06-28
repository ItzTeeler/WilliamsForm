namespace BackendRedo.Models;

// Think of this as the User Model
    public class UserModel
    {
        public int ID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? DOB { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public string? Email{ get; set; }
        public bool? isAdmin { get; set; }
        public bool? isDeleted { get; set; }
        

        public UserModel()
        {
            
        }
    }
