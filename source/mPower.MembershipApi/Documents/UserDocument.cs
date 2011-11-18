using System;
using System.Collections.Generic;
using mPower.MembershipApi.Enums;

namespace mPower.MembershipApi.Documents
{
    public class UserDocument
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string AuthToken { get; set; }

        public string ResetPasswordToken { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public List<UserPermissionEnum> Permissions { get; set; }
    }
}
