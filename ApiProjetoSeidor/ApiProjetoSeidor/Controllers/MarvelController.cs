using Connection;
using MarvelApi;
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
    public class MarvelController : ControllerBase
    {
        private MarvelApiBusiness marvelApiBusiness_;


        /// <summary>
        /// Creates new marvel api config
        /// </summary>
        /// <response code="200">Sucessful cadastration</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     POST /api/marvel/v1/new/config
        ///     {
        ///        		"privateKey": "fdfeeb6646957e51330eee9f221628ded63979c0",
        ///             "publicKey": "3d0249f2242b47171aa5bfbe2eb5dfa7"
        ///     }
        ///     
        ///
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(Package), 200)]
        [Route("v1/new/config")]
        [HttpPost]
        public IActionResult NewMarvelApiConfiguration(MarvelApiConfiguration marvelApiConfiguration)
        {
            try
            {

                string token = HttpContext.Request.Headers["AUTHORIZATION"];

                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.CreateNewApiConfig(marvelApiConfiguration);

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
        /// Gets Api Config By user logged
        /// </summary>
        /// <response code="200">returns Apiconfig with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/marvel/config
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(MarvelApiConfiguration), 200)]
        [Route("v1/config")]
        [HttpGet]
        public IActionResult GetApiConfigByUser()
        {
            try
            {
                string token = HttpContext.Request.Headers["AUTHORIZATION"];

                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.GetApiConfig();

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
        /// Gets marvel's hero by name
        /// </summary>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/marvel/hero/{name}
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(MarvelPayload), 200)]
        [Route("v1/hero/{name}")]
        [HttpGet]
        public IActionResult GetHeroByName(string name)
        {
            try
            {
                string token = HttpContext.Request.Headers["AUTHORIZATION"];

                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.GetHeroByName(name);

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
        /// Gets marvel's hero by name, with token within the url
        /// </summary>
        /// <returns>
        /// 
        /// ok
        /// 
        /// </returns>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/marvel/hero/{name}/{token}
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(MarvelPayload), 200)]
        [Route("v1/hero/{name}/{token}")]
        [HttpGet]
        public IActionResult GetHeroByName(string name,string token)
        {
            try
            {
                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.GetHeroByName(name);

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
        /// Gets marvel's series by name
        /// </summary>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/marvel/series/{name}
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(MarvelPayload), 200)]
        [Route("v1/series/{name}")]
        [HttpGet]
        public IActionResult GetSeriesByName(string name)
        {
            try
            {
                string token = HttpContext.Request.Headers["AUTHORIZATION"];

                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.GetSeriesByName(name);

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
        /// Gets marvel's series by name, with token within the url
        /// </summary>
        /// <returns>
        /// 
        /// ok
        /// 
        /// </returns>
        /// <response code="200">returns marvel payload with data found</response>
        /// <response code="400">BadRequest, validation errors, missing expected fields</response>
        /// <response code="401">Unauthorized, invalid access token or invalid marvel keys</response>
        /// <response code="500">Critical Unexpected Error</response>
        /// /// <remarks>
        /// Exemple:
        ///
        ///     GET /api/marvel/hero/{name}/{token}
        /// </remarks>
        /// 

        [ProducesResponseType(typeof(MarvelPayload), 200)]
        [Route("v1/series/{name}/{token}")]
        [HttpGet]
        public IActionResult GetSeriesByName(string name, string token)
        {
            try
            {
                marvelApiBusiness_ = new MarvelApiBusiness(token);

                Package package = marvelApiBusiness_.GetSeriesByName(name);

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
