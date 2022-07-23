﻿using CustomImageProvider.Tests;
using Newtonsoft.Json;
using Samp.Core.Results.Abstracts;
using Samp.Identity.API.Models.Dto;
using Samp.Identity.API.Models.Requests;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Samp.Tests.Controllers
{
    public class UsersControllerTests : MainControllerTests<Samp.Identity.API.Startup>
    {
        public UsersControllerTests(CustomWebApplicationFactory<Samp.Identity.API.Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task<ResponseModel<UserDto>> Delete()
        {
            var obj = await Post();
            obj.Results.ShouldNotBeEmpty();
            obj.Results.Count.ShouldBeEquivalentTo(1);

            var response = await client.DeleteAsync($"api/Users/{obj.Results.First().Id}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Errors.ShouldBeNull();
            data.Results.ShouldNotBeNull();
            data.Results.First().Name.ShouldBe(obj.Results.First().Name);
            data.Results.First().Surname.ShouldBe(obj.Results.First().Surname);
            data.Results.First().Id.ShouldBe(obj.Results.First().Id);

            return data;
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public async Task DeleteById_NotFound(Guid userId)
        {
            var response = await client.DeleteAsync($"api/Users/{userId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.ShouldBeNull();
            data.Errors.ShouldBeNull();
        }

        [Fact]
        public async Task GetAll()
        {
            var response = await client.GetAsync("api/Users");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Errors.ShouldBeNull();
            data.Results.ShouldNotBeNull();
            data.Results.Count.ShouldBeGreaterThan(0);
        }

        [Theory]
        [InlineData(null)]
        public async Task<ResponseModel<UserDto>> GetById(Guid? userId = null)
        {
            if (!userId.HasValue)
            {
                var user = await Search("a");
                userId = user.Results.First().Id;
            }

            var response = await client.GetAsync($"api/Users/{userId}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.ShouldNotBeEmpty();

            return data;
        }

        [Theory]
        [InlineData("Alavere")]
        [InlineData("Falan")]
        public async Task GetByName(string userName)
        {
            var response = await client.GetAsync($"api/Users/Name/{userName}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData("Dalavere")]
        [InlineData("FILAN")]
        public async Task GetBySurname(string userSurname)
        {
            var response = await client.GetAsync($"api/Users/Surname/{userSurname}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task<ResponseModel<UserDto>> Post()
        {
            UserCreateModel obj = new UserCreateModel()
            {
                Name = "testName",
                Surname = "testSurname",
                Email = "xunit@test.com",
                Username = "xunit1",
                Password = "xunit1"
            };

            var response = await client.PostAsync("api/Users",
                new StringContent(JsonConvert.SerializeObject(obj, jsonSerializerSettings), Encoding.UTF8, "application/json"));

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.First().Name.ShouldBe(obj.Name);
            data.Results.First().Surname.ShouldBe(obj.Surname);

            return data;
        }

        [Fact]
        public async Task Put()
        {
            ResponseModel<UserDto> obj = await Post();

            UserUpdateModel userUpdateModel = new UserUpdateModel()
            {
                Name = "updated-" + obj.Results.First().Name,
                Surname = "updated-" + obj.Results.First().Surname,
                Email = "updated-" + obj.Results.First().Email,
            };

            var response = await client.PutAsync($"api/Users/{obj.Results.First().Id}",
                new StringContent(JsonConvert.SerializeObject(userUpdateModel, jsonSerializerSettings), Encoding.UTF8, "application/json"));
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);
            data.Stats.RId.ShouldNotBeNull();

            var userData = await GetById(obj.Results.First().Id);
            userData.Stats.RId.ShouldNotBeNull();
            userData.Results.Count.ShouldBe(1);
            userData.Results.First().Name.ShouldBe(userUpdateModel.Name);
            userData.Results.First().Surname.ShouldBe(userUpdateModel.Surname);
            userData.Results.First().Email.ShouldBe(userUpdateModel.Email);
        }

        [Theory]
        [InlineData("a")]
        public async Task<ResponseModel<UserDto>> Search(string queryString)
        {
            var response = await client.GetAsync($"api/Users/Search?q={queryString}");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var data = ConvertResponse<ResponseModel<UserDto>>(response);

            data.Stats.RId.ShouldNotBeNull();
            data.Results.ShouldNotBeEmpty();

            return data;
        }
    }
}