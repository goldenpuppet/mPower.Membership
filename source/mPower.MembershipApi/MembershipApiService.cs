using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using mPower.MembershipApi.Documents;
using Newtonsoft.Json;

namespace mPower.MembershipApi
{
    public enum MembershipApiMethod
    {
        Test,
        CreateUser,
        LogIn,
        Activate,
        DeActivate,
        ChangePassword,
        LogInByToken,
        GetResetPasswodToken,
        ResetPassword,
        DeleteUser,
        AddAuthenticationQuestion,
        GetAuthenticationQuestion,
        ValidateAuthenticationQuestion,
        GetUserByUsername,
        HasAccess
    }

    public class MembershipApiService
    {
        public MembershipApiService(string apiPrivateKey, string apiBaseUrl)
        {
            _apiPrivateKey = apiPrivateKey;
            _apiBaseUrl = apiBaseUrl;
        }

        public ApiResponseObject Test()
        {
            return ExecuteAction(MembershipApiMethod.Test, new NameValueCollection());
        }

        public UserDocument CreateUser(string firstName, string lastName, string email, string userName, string password)
        {
            var requestParameters = new NameValueCollection();
            requestParameters.Add("FirstName", firstName);
            requestParameters.Add("LastName", lastName);
            requestParameters.Add("Email", email);
            requestParameters.Add("UserName", userName);
            requestParameters.Add("Password", password);

            var result = ExecuteAction(MembershipApiMethod.CreateUser, requestParameters);

            var user = JsonConvert.DeserializeObject<UserDocument>(result.data);

            return user;
        }

        public UserDocument LogIn(string userName, string password)
        {
            var requestParameters = new NameValueCollection { { "userName", userName }, { "password", password } };

            var result = ExecuteAction(MembershipApiMethod.LogIn, requestParameters);
            var user = JsonConvert.DeserializeObject<UserDocument>(result.data);

            return user;
        }

        public UserDocument LogInByToken(string authToken)
        {
            var requestParameters = new NameValueCollection { { "authToken", authToken } };

            var result = ExecuteAction(MembershipApiMethod.LogInByToken, requestParameters);
            var user = JsonConvert.DeserializeObject<UserDocument>(result.data);

            return user;
        }

        public void Activate(string userId)
        {
            var requestParameters = new NameValueCollection { { "userId", userId } };

            ExecuteAction(MembershipApiMethod.Activate, requestParameters);
        }

        public void DeActivate(string userId)
        {
            var requestParameters = new NameValueCollection { { "userId", userId } };

            ExecuteAction(MembershipApiMethod.DeActivate, requestParameters);
        }

        public void ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var requestParameters = new NameValueCollection { { "userId", userId }, { "oldPassword", oldPassword }, { "newPassword", newPassword } };

            ExecuteAction(MembershipApiMethod.ChangePassword, requestParameters);
        }

        public string GetResetPasswodToken(string email)
        {
            var requestParameters = new NameValueCollection { { "email", email } };

            var result = ExecuteAction(MembershipApiMethod.GetResetPasswodToken, requestParameters);

            var token = JsonConvert.DeserializeObject<string>(result.data);

            return token;
        }

        public string ResetPassword(string resetToken)
        {
            var requestParameters = new NameValueCollection { { "resetToken", resetToken } };

            var result = ExecuteAction(MembershipApiMethod.ResetPassword, requestParameters);

            var newPassword = JsonConvert.DeserializeObject<string>(result.data);

            return newPassword;
        }

        public void DeleteUser(string userId)
        {
            var requestParameters = new NameValueCollection { { "userId", userId } };

            ExecuteAction(MembershipApiMethod.DeleteUser, requestParameters);
        }

        public void AddAuthenticationQuestion(string userId, string question, string answer)
        {
            var requestParameters = new NameValueCollection { { "userId", userId }, { "question", question }, { "answer", answer } };

            ExecuteAction(MembershipApiMethod.AddAuthenticationQuestion, requestParameters);
        }

        public string GetAuthenticationQuestion(string userId)
        {
            var requestParameters = new NameValueCollection { { "userId", userId } };

            var result = ExecuteAction(MembershipApiMethod.GetAuthenticationQuestion, requestParameters);

            var question = JsonConvert.DeserializeObject<string>(result.data);

            return question;
        }

        public bool ValidateAuthenticationQuestion(string userId, string answer)
        {
            var requestParameters = new NameValueCollection { { "userId", userId }, { "answer", answer } };

            var result = ExecuteAction(MembershipApiMethod.ValidateAuthenticationQuestion, requestParameters);

            var isValid = JsonConvert.DeserializeObject<bool>(result.data);

            return isValid;
        }

        public UserDocument GetUserByUsername(string userName)
        {
            var requestParameters = new NameValueCollection { { "userName", userName } };

            var result = ExecuteAction(MembershipApiMethod.GetUserByUsername, requestParameters);
            var user = JsonConvert.DeserializeObject<UserDocument>(result.data);

            return user;
        }

        public bool HasAccess(string userId, params UserPermissionEnum[] permissions)
        {
            var permissionsString = String.Join(",", permissions.Select(x => x.ToString()));

            var requestParameters = new NameValueCollection { { "permissions", permissionsString }, { "userId", userId } };

            var result = ExecuteAction(MembershipApiMethod.HasAccess, requestParameters);

            var isValid = JsonConvert.DeserializeObject<bool>(result.data);

            return isValid;
        }

        #region private part

        private readonly string _apiPrivateKey;
        private readonly string _apiBaseUrl;

        private Uri BuildPathToTheAction(MembershipApiMethod method)
        {
            return new Uri(String.Format("{0}/{1}", _apiBaseUrl, method));
        }

        private ApiResponseObject ExecuteAction(MembershipApiMethod method, NameValueCollection parameters)
        {
            parameters.Add("key", _apiPrivateKey);
            var queryString = ToQueryString(parameters);
            var uri = BuildPathToTheAction(method);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.ASCII.GetBytes(queryString);
            request.ContentLength = bytes.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            string responseText;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var answerReader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = answerReader.ReadToEnd();
                }
            }

            var item = JsonConvert.DeserializeObject<ApiResponseObject>(responseText);
            

            //I am not throw exception for the ModelState errors
            if (item.status == ApiResponseStatusEnum.Error && item.error_code != MembershipApiErrorCodesEnum.ModelStateErrors)
            {
                throw new Exception(String.Format("Remote server returned following error code: {0}({1}), {2}",
                                                                                                        item.status,
                                                                                                        item.error_code,
                                                                                                        item.log
                                                                                                        ));
            }

            return item;
        }

        private static string ToQueryString(NameValueCollection nvc)
        {
            return string.Join("&", Array.ConvertAll(nvc.AllKeys, key => string.Format("{0}={1}", key, nvc[key])));
        }

        #endregion
    }
}
