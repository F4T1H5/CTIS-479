using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class GenreResponse : Response
    {
        public string Name { get; set; }

        [DisplayName("Movie Count")]
        public int MovieCount { get; set; }

        public string Movies { get; set; }
    }
}
