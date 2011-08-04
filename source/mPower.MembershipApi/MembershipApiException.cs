using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mPower.MembershipApi.Enums;

namespace mPower.MembershipApi
{
    public class MembershipApiException : Exception
    {
        public MembershipApiErrorCodesEnum ErrorCode { get; set; }
        public string Log { get; set; }

        public MembershipApiException(string message, MembershipApiErrorCodesEnum errorCode, string log)
            : base(message)
        {
            ErrorCode = errorCode;
            Log = log;
        }

        public override string ToString()
        {
            return String.Format("Membershi api excetion. Code: {0}. Log: {1}", ErrorCode, Log);
        }
    }
}
