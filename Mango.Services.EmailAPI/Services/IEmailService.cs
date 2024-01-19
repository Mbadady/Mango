using System;
using Mango.Services.EmailAPI.Models.DTO;

namespace Mango.Services.EmailAPI.Services
{
	public interface IEmailService
	{

		Task EmailCartAndLog(CartDTO cartDTO);
		Task RegisterEmailAndLog(string emailAddress);
	}
}

