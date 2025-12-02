using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class DirectorResponse : Response
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsRetired { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Retired")]
        public string IsRetiredF { get; set; }
    }
}
