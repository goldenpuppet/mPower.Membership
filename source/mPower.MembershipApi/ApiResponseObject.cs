namespace mPower.MembershipApi
{
    public class ApiResponseObject
    {
        public ApiResponseStatusEnum status { get; set; }
        public string data { get; set; }
        public string log { get; set; }
        public MembershipApiErrorCodesEnum error_code { get; set; }
    }
}