using System;

namespace mPower.MembershipApi
{
    public class MembershipApiAccessDeniedException : Exception
    {
        public MembershipApiAccessDeniedException()
            : base()
        {
        }

        public MembershipApiAccessDeniedException(string message)
            : base(message)
        {
        }
    }
}
