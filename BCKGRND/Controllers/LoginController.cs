using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCKGRND.Models;
using Newtonsoft.Json;
using System.Text;

namespace BCKGRND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly DBContext _context;

        public LoginController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Post([FromBody]User value)
        {
            if(_context.Users.Any(user => user.UserMail.Equals(value.UserMail)))
            {
                User user = _context.Users.Where(user => user.UserMail.Equals(value.UserMail)).First();

                var postHashPassword = Convert.ToBase64String(
                    Utils.Common.SaltHashPassword(
                        Encoding.ASCII.GetBytes(value.UserPass),
                        Convert.FromBase64String(user.Salt)));

                if (postHashPassword.Equals(user.UserPass))
                {
                    //return JsonConvert.SerializeObject("Logged in successfully");
                    return JsonConvert.SerializeObject(user);
                }
                else
                {
                    return JsonConvert.SerializeObject("Wrong password");
                }
            }
            else
            {
                return JsonConvert.SerializeObject("User doesn't exist");
            }
        }
    }
}
