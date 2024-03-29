﻿using System;
using mPower.MembershipApi.Documents;
using mPower.MembershipApi.Enums;

namespace mPower.MembershipApi.Examples
{
    class Program
    {
        private static string _apiPrivateKey = "ee8f6240b612168390eef1abe983288d";
        private static string _apiBaseUrl = "http://staging.mpowering.com/api/membership";
        private static string _apiBaseUrlLocal = "http://localhost:8080/api/membership";


        static void Main(string[] args)
        {
            var membershipService = new MembershipApiService(_apiPrivateKey, _apiBaseUrl);
            var token1 = membershipService.GetResetPasswodToken("demo");

            var newPassword1 = membershipService.ResetPassword(token1);

            UserDocument user;
            var userId = String.Empty;
            var email = "test@test.com";
            var userName = Guid.NewGuid().ToString();
            user = membershipService.CreateUser("TestFirst", "TestLast", email, userName, "asd123");
            userId = user.Id;
            user = membershipService.LoginByUserIdAndPassword(userId, "asd123");

            var hasAccess = membershipService.HasAccess(userId, UserPermissionEnum.ViewPfm);
            userId = user.Id;
            user = membershipService.LogIn(userName, "asd123");
            membershipService.Activate(userId);

            membershipService.DeActivate(userId);

            user = membershipService.LogInByToken(user.AuthToken);

            membershipService.ChangePassword(userId, "asd123", "asd321");

            var token = membershipService.GetResetPasswodToken(email);

            var newPassword = membershipService.ResetPassword(token);

            membershipService.AddAuthenticationQuestion(userId, "Where is your mother born?", "Zimbabve");
            var question = membershipService.GetAuthenticationQuestion(userId);
            bool isValid = membershipService.ValidateAuthenticationQuestion(userId, "Zimbabve");
            user = membershipService.GetUserByUsername(userName);
            membershipService.DeleteUser(userId);

            int i = 1;
        }
    }
}
