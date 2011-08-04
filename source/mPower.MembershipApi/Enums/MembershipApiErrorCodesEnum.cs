
namespace mPower.MembershipApi.Enums
{
    public enum MembershipApiErrorCodesEnum
    {
        None = 0,
        ModelStateErrors = 1,
        UserNameExists = 2,
        UserNotFound = 3,
        InvalidApiKey = 4,
        InvalidChangePasswordToken = 5,
        ApiCallServerError = 6
    }
}