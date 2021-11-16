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

        [Route("new/config")]
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

        [Route("hero/{name}")]
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

    }
}
