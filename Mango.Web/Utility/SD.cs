using System;
namespace Mango.Web.Utility
{
	public class SD
	{
		public static string CouponAPIBase { get; set; }
		public static string AuthAPIBase { get; set; }
		public static string ProductAPIBase { get; set; }
		public static string CartAPIBase { get; set; }


		public const string RoleAdmin = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookies = "JwtToken";
        public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}

