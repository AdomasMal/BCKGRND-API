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
        private readonly DBContext _context;

        public RegisterController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Post([FromBody]User value)
        {
            string responseBody;
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
                    responseBody = JsonConvert.SerializeObject("Registered successfully");
                }
                catch(Exception ex)
                {
                    responseBody = JsonConvert.SerializeObject(ex.Message);
                }
            }
            else
            {
                responseBody = JsonConvert.SerializeObject("E-mail is already registered");
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }
    }
}
