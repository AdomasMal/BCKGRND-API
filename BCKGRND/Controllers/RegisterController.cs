using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// For registering new users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        "UserMail": "test@mail.com",
        ///        "UserName": "user",
        ///        "UserPass": "pass1234"
        ///     }
        /// </remarks>
        /// <returns> Returns status message</returns>
        /// <response code="200">Returns either "Registered successfully" or "E-mail is already registered"</response>
        /// <response code="400">On execption</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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
            Response.ContentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(responseBody);
            return responseBody;
        }
    }
}
