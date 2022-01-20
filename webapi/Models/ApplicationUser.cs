using CodeFirst;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Nickname { get; set; }

        static public List<string> getusernames(IEnumerable<ApplicationUser> users)
        {

            List<string> tmp = new List<string>();
            foreach (var item in users)
            {
                tmp.Add(item.UserName);
            }
            return tmp;
        }
    }
}
