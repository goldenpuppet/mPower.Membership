using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mPower.MembershipApi.Documents;

namespace mPower.MembershipApi
{
    class Program
    {
        private static string _apiPrivateKey = "F89DA1C7DC2B40319802BCF4F6B2C121";
        private static string _apiBaseUrl = "http://staging.mpowering.com/api/membership";
        private static string _apiBaseUrlLocal = "http://localhost:8080/api/membership";

        static void Main(string[] args)
        {
            var membershipService = new MembershipApiService(_apiPrivateKey, _apiBaseUrlLocal);
            UserDocument user;
            var userId = String.Empty;
            var userName = Guid.NewGuid().ToString();
            user = membershipService.CreateUser( "TestFirst", "TestLast", "an.orsich@gmail.com", userName, "asd123");
            userId = user.Id;
            user = membershipService.LogIn(userName, "asd123");
            membershipService.Activate(userId);
            
            membershipService.DeActivate(userId);
            
            user = membershipService.LogInByToken(user.AuthToken);
            
            membershipService.ChangePassword(userId, "asd123", "asd321");

            var token = membershipService.GetResetPasswodToken("an.orsich@gmail.com");

            var newPassword = membershipService.ResetPassword(token);

            membershipService.AddAuthenticationQuestion(userId, "Where is your mother born?", "Novogrudok");
            var question = membershipService.GetAuthenticationQuestion(userId);
            bool isValid = membershipService.ValidateAuthenticationQuestion(userId, "Novogrudok");
            user = membershipService.GetUserByUsername(userName);
            membershipService.DeleteUser(userId);

            int i = 1;
        }
    }
}
