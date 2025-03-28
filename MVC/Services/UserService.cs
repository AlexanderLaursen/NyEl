﻿using Common.Dtos.Consumer;
using Common.Dtos.User;
using Common.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class UserService : CommonApiService, IUserService
    {
        public const string USERS = "/users";
        public UserService(HttpClient httpClient, ILogger<CommonApiService> logger, IConfiguration configuration) : base(httpClient, logger, configuration)
        {
        }

    }
}
