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
    public class RegisterController : Controller
    {
        private readonly UserContext _context;

        public RegisterController(UserContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Post([FromBody]User value)
        {
            if(!_context.Users.Any(user => user.UserMail.Equals(value.UserMail)))
            {
                User user = new User();
                user.UserName = value.UserName;
                user.UserMail = value.UserMail;
                user.Salt = Convert.ToBase64String(Utils.Common.GetRandomSalt(16));
                user.UserPass = Convert.ToBase64String(Utils.Common.SaltHashPassword(
                    Encoding.ASCII.GetBytes(value.UserPass),
                    Convert.FromBase64String(user.Salt)));

                try
                {
                    _context.Add(user);
                    _context.SaveChanges();
                    return JsonConvert.SerializeObject("Registered successfully");
                }
                catch(Exception ex)
                {
                    return JsonConvert.SerializeObject(ex.Message);
                }
            }
            else
            {
                return JsonConvert.SerializeObject("E-mail is already registered");
            }
        }
    }
}
