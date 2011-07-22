using System.Web.Mvc;
using System.Web;
using System;
using mPower.MembershipApi.Enums;

namespace mPower.MembershipApi
{
    public class PermissionsAttribute : ActionFilterAttribute
    {
        private readonly UserPermissionEnum[] _permissions;


        public PermissionsAttribute(params UserPermissionEnum[] permissions)
        {
            this._permissions = permissions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sessionUserId = HttpContext.Current.Session["UserId"];
            var userId = sessionUserId == null ? String.Empty : sessionUserId.ToString();
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User id not found in Session. User id should be in Session with name 'UserId'.");
            }

            var membershipService = new MembershipApiService(Config.Instance.ApiPrivateKey, Config.Instance.ApiBaseUrl);

            var hasAccess = membershipService.HasAccess(userId, _permissions);

            if (!hasAccess)
                throw new MembershipApiAccessDeniedException("Access denied");
        }
    }
}
