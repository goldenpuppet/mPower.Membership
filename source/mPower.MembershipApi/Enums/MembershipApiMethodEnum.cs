namespace mPower.MembershipApi.Enums
{
    public enum MembershipApiMethodEnum
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
        HasAccess,
        LoginByUserIdAndPassword
    }
}
