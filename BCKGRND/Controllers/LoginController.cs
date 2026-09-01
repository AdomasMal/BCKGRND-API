using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// For logging in
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "UserMail": "test@mail.com",
        ///        "UserPass": "pass1234"
        ///     }
        /// </remarks>
        /// <returns> Returns user object or status message</returns>
        /// <response code="200">Returns user object or either "Wrong password" or "User doesn't exist" messages</response>
        /// <response code="400">On execption</response>
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
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
            Response.ContentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(responseBody);
            return responseBody;
        }

        /// <summary>
        /// For changing user password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Login/1/pass5678
        ///     {
        ///        "UserMail": "test@mail.com",
        ///        "UserPass": "pass1234"
        ///     }
        /// </remarks>
        /// <returns> Returns user object or status message</returns>
        /// <response code="200">Returns user object or either "Wrong password" or "User doesn't exist" messages</response>
        /// <response code="400">On execption</response>
        [HttpPut("{id}/{newPass}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
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
            Response.ContentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(responseBody);
            return responseBody;
        }

        /// <summary>
        /// For deleting user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Login/1
        /// </remarks>
        /// <returns> Returns user object or status message</returns>
        /// <response code="200">Returns either "User deleted" or "User does not exist"</response>
        /// <response code="400">On execption</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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
            Response.ContentLength = System.Text.ASCIIEncoding.UTF8.GetByteCount(responseBody);
            return responseBody;
        }
    }
}
