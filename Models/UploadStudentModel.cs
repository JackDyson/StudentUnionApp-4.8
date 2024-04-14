using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentUnionApp.Models
{
    public class UploadStudentModel
    {
        public string ClubName { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string PreferredName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Agreement { get; set; }
        public bool Training { get; set; }
        public bool Membership { get; set; }
        public bool Food { get; set; }
    }
}