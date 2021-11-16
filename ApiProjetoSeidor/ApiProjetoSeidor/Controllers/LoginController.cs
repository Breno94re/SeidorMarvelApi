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

        /// <summary>
        /// Login into application with valid account
        /// </summary>
        /// <returns>Token acess</returns>
        /// <response code="200">returns user token for that session, with 48h lifespan </response>
        /// <response code="401">Unauthorized acess, wrong credentials </response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     POST /api/login/vi
        ///     {
        ///        "email": "john@mail.com",
        ///        "password": "1cc9f57c052e11bf7785a3b2037f6a241fa6831ab64cf929bf993a9b3139b830268df567e623eda8f04768c82e2cce206c37233522dcaf4a834ad87f79c69527"
        ///     }
        ///     ** password needs to be in sha512 format **
        ///
        /// </remarks>
        /// 

        [Route("v1")]
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

        /// <summary>
        /// Creates new account
        /// </summary>
        /// <returns>Ok</returns>
        /// <response code="200">Sucessful cadastration</response>
        /// <response code="400">BadReques, validation errors, missing expected fields</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     POST /api/login/new/user
        ///     {
        ///        	"email":"user@mail.com",
        ///        	"name":"john someone",
        ///        	"password":"1cc9f57c052e11bf7785a3b2037f6a241fa6831ab64cf929bf993a9b3139b830268df567e623eda8f04768c82e2cce206c37233522dcaf4a834ad87f79c69527"
        ///     }
        ///     ** password needs to be in sha512 format **
        ///
        /// </remarks>
        /// 

        [Route("v1/new/user")]
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
