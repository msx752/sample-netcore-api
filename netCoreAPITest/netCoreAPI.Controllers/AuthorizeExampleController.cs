﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netCoreAPI.Controllers.Base;
using netCoreAPI.Static.Services;

namespace netCoreAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthorizeExampleController : MainController
    {
        public AuthorizeExampleController(ISharedRepository myRepository, IMapper mapper) : base(myRepository, mapper)
        {
        }

        /// <summary>
        /// require authorization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}