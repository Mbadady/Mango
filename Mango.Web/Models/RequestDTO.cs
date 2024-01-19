using System;
//using Mango.Web.Utility.SD;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Models
{
	public class RequestDTO
	{
		public ApiType ApiType { get; set; } = Utility.SD.ApiType.GET;
		public string Url { get; set; }
		public object Data { get; set; }
		public string AccessToken { get; set; }
	}
}

