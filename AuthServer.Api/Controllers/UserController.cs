﻿using AuthServer.Core.Dto;
using AuthServer.Core.Services;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService= userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)//[FromBody]CreateUserDto createUserDto
        {
           
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));

        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }
    }
}
