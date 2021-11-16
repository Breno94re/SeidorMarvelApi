using Connection;
using Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Utility;

namespace ApiProjetoSeidor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private LoginBusiness loginBusiness_;

        public LoginController()
        {
            loginBusiness_ = new LoginBusiness();
        }

        [HttpPost]
        public IActionResult Login(LoginAuth login)
        {
            try
            {
                login.IpAdress = HttpContext.Request.Host.Value;
                Package package = loginBusiness_.Login(login);

                if (!package.HasNotifications())
                {
                    return Ok(package);
                }
                else
                {
                    return BadRequest(package);
                }
            }
            catch (UnauthorizedConnection j)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Route("new/user")]
        [HttpPost]
        public IActionResult NewUser(User user)
        {
            try
            {
                Package package = loginBusiness_.CreateNewUser(user);

                return StatusCode(package.HttpCode, package);

            }
            catch (UnauthorizedConnection j)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

       

    }

}
