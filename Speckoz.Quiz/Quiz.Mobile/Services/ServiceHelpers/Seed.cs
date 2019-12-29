﻿using Quiz.Mobile.Properties;
using Quiz.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Quiz.Services.Helpers
{
    public static class Seed
    {
        public static List<UserModel> SeedUsers()
        {
            return new List<UserModel>
            {
                new UserModel(){ Login = "admin", Password = "admin" },
                new UserModel(){ Login = "marco", Password = "123" },
                new UserModel(){ Login = "heart", Password = "password" },
                new UserModel(){ Login = "gmail", Password = "777"},
                new UserModel(){ Login = "specko", Password = "2020"},
                new UserModel(){ Login = "javaf", Password = "javafx"}
            };
        }

        public static List<QuestionModel> SeedQuestions()
        {
            return JsonSerializer.Deserialize<List<QuestionModel>>(Encoding.UTF8.GetString(Resources.Questions));
        }
    }
}