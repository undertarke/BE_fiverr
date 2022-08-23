using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class NguoiDungViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
       
        public string Birthday { get; set; }
        public string Avatar { get; set; }
        public bool Gender { get; set; }
        public string Role { get; set; }
        public List<string> Skill { get; set; }
        public List<string> Certification { get; set; }
        public List<string> BookingJob { get; set; }

    }
}