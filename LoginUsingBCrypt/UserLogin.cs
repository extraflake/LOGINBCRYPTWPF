using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsingBCrypt
{
    public class UserLogin
    {
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public string Authorities { get; set; }
        public string Password { get; set; }
    }
}
