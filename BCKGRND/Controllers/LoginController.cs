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
using Newtonsoft.Json.Linq;

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
            string responseBody;
            if (_context.Users.Any(user => user.UserMail.Equals(value.UserMail)))
            {
                User user = _context.Users.Where(user => user.UserMail.Equals(value.UserMail)).First();

                var postHashPassword = Convert.ToBase64String(
                    Utils.Common.SaltHashPassword(
                        Encoding.ASCII.GetBytes(value.UserPass),
                        Convert.FromBase64String(user.Salt)));

                if (postHashPassword.Equals(user.UserPass))
                {
                    responseBody = JsonConvert.SerializeObject(user);
                }
                else
                {
                    responseBody = JsonConvert.SerializeObject("Wrong password");
                }
            }
            else
            {
                responseBody = JsonConvert.SerializeObject("User doesn't exist");
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }

        [HttpPut("{id}/{newPass}")]
        public string Get(int id, string newPass, [FromBody] User value)
        {
            string responseBody;
            try
            {
                if (_context.Users.Any(user => user.UserMail.Equals(value.UserMail)))
                {
                    User user = _context.Users.Where(user => user.UserMail.Equals(value.UserMail)).First();

                    var postHashPassword = Convert.ToBase64String(
                        Utils.Common.SaltHashPassword(
                            Encoding.ASCII.GetBytes(value.UserPass),
                            Convert.FromBase64String(user.Salt)));

                    if (postHashPassword.Equals(user.UserPass))
                    {
                        user.Salt = Convert.ToBase64String(Utils.Common.GetRandomSalt(16));
                        user.UserPass = Convert.ToBase64String(Utils.Common.SaltHashPassword(
                            Encoding.ASCII.GetBytes(newPass),
                            Convert.FromBase64String(user.Salt)));
                        _context.SaveChanges();
                        responseBody = JsonConvert.SerializeObject(user);
                    }
                    else
                    {
                        responseBody = JsonConvert.SerializeObject("Wrong password");
                    }
                }
                else
                {
                    responseBody = JsonConvert.SerializeObject("User doesn't exist");
                }
            }
            catch(Exception ex)
            {
                responseBody = JsonConvert.SerializeObject(ex);
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }

        [HttpDelete("{id:int}")]
        public string Delete(int id)
        {
            string responseBody;
            try
            {
                if(_context.Users.Any(user => user.ID.Equals(id)))
                {
                    User user = _context.Users.FirstOrDefault(user => user.ID.Equals(id));
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    responseBody = JsonConvert.SerializeObject("User deleted");
                }
                else
                {
                    responseBody = JsonConvert.SerializeObject("User does not exist");
                }
            }
            catch (Exception ex)
            {
                responseBody = JsonConvert.SerializeObject(ex.Message);
            }
            Response.ContentLength = responseBody.Length;
            return responseBody;
        }
    }
}
