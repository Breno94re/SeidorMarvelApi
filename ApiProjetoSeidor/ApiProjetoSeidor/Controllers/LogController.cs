using Connection;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace ApiProjetoSeidor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        LoggerBusiness loggerBusiness_;
        /// <summary>
        /// Gets System Log by user logged in
        /// </summary>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/log/v1
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(List<Log>), 200)]
        [Route("v1")]
        [HttpGet]
        public IActionResult ListLogByUser()
        {
            try
            {
                string token = HttpContext.Request.Headers["AUTHORIZATION"];

                loggerBusiness_ = new LoggerBusiness(token);

                Package package = loggerBusiness_.ListLogByUser();

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

        /// <summary>
        /// Gets System Log by user logged in token within url
        /// </summary>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/log/v1/{token}
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(List<Log>), 200)]
        [Route("v1/{token}")]
        [HttpGet]
        public IActionResult ListLogByUser(string token)
        {
            try
            {

                loggerBusiness_ = new LoggerBusiness(token);

                Package package = loggerBusiness_.ListLogByUser();

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
