namespace BackendRedo.Models;

    public class FormModel
    {
        public int ID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address{ get; set; }
        public string? Phonenumber{ get; set; }
        public string? Password{ get; set; }
        public string? Email{ get; set; }
        public string? DOB{ get; set; }
        

        public FormModel()
        {
            
        }
    }
