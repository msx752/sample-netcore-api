﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samp.Core.Interfaces.Repositories;
using Samp.Core.Model.Base;
using Samp.Identity.API.Helpers;
using Samp.Identity.API.Models.Dto;
using Samp.Identity.API.Models.Requests;
using Samp.Identity.Core.Migrations;
using Samp.Identity.Database.Entities;
using Samp.Result;
using System.Security.Claims;

namespace Samp.Identity.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class TokenController : BaseController
    {
        private readonly IUnitOfWork<IdentityDbContext> repository;
        private readonly ITokenHelper tokenHelper;

        public TokenController(
            IMapper mapper
            , IUnitOfWork<IdentityDbContext> repository
            , ITokenHelper tokenHelper)
            : base(mapper)
        {
            this.repository = repository;
            this.tokenHelper = tokenHelper;
        }

        [HttpPost]
        public ActionResult Post([FromForm] TokenRequest model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResponse(ModelState.Values.SelectMany(f => f.Errors).Select(f => f.ErrorMessage));
            }

            if (model.grant_type.Equals("password", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return new BadRequestResponse("Username and Password fields can not be empty.");
                }

                var user = repository.Table<UserEntity>()
                    .FirstOrDefault(f => f.Username.Equals(model.Username) && f.Password.Equals(model.Password));

                if (user == null)
                {
                    return new UnauthorizedResponse("invalid credentials.");
                }

                var claims = new[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.Id.ToString()),
                };
                TokenDto response = tokenHelper.Authenticate(user, claims);
                response.User = mapper.Map<UserDto>(user);
                return new OkResponse(response);
            }
            return new BadRequestResponse($"invalid grant_type value:'{model.grant_type}'");
        }
    }
}