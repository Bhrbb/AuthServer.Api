﻿using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserServices : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        public UserServices(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName
               

            };
            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded) { 
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }
            return Response<UserAppDto>.Succes(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404, true);

            }
            return Response<UserAppDto>.Succes(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

        }
    }
}
