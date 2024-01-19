using System;
using System.Text;
using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.EmailAPI.Services
{
	public class EmailService : IEmailService
	{
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/> Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDTO.CartHeaderDTO.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");

            foreach (var item in cartDTO.CartDetailsDTO)
            {
                message.Append("<li>");
                message.Append(item.ProductDTO.Name + " * " + item.Count);
                message.Append("</li>");
            }

            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeaderDTO.Email);

        }

        public async Task RegisterEmailAndLog(string emailAddress)
        {
            string message = "User registration successfull. <br/> Email : " + emailAddress;

            await LogAndEmail(message, "victorsomtochukwu@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string emailAddress)
            {
                try
                {
                EmailLogger emailLog = new EmailLogger()
                {
                    Message = message,
                    Email = emailAddress,
                    EmailSent = DateTime.Now
                };

                await using var _db = new AppDbContext(_dbOptions);

                await _db.EmailLoggers.AddAsync(emailLog);

                await _db.SaveChangesAsync();

                return true;
                }
                catch (Exception ex)
                {
                return false;
                }
            }
    }
}

