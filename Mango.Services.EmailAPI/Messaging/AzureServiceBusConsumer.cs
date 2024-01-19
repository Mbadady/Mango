using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.DTO;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;

namespace Mango.Services.EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _registerUserQueue;


        // if we want to listen to a queue
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;


        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
		{
            _configuration = configuration;
            _emailService = emailService;
            _serviceBusConnectionString = _configuration.GetValue<string>("EmailShoppingCartConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterQueue");


            var client = new ServiceBusClient(_serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
            _registerUserProcessor = client.CreateProcessor(_registerUserQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            _registerUserProcessor.ProcessMessageAsync += OnRegisterUserRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;


            await _emailCartProcessor.StartProcessingAsync();
            await _registerUserProcessor.StartProcessingAsync();

        }

        
        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();

            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();

            await _registerUserProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());

            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO obj = JsonConvert.DeserializeObject<CartDTO>(body);

            try
            {
                // try to log to email

                _emailService.EmailCartAndLog(obj);
                await arg.CompleteMessageAsync(arg.Message);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnRegisterUserRequestReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;

            var body = Encoding.UTF8.GetString(message.Body);

            var emailAddress = JsonConvert.DeserializeObject<string>(body);

            try
            {
                // try to log to email

                await _emailService.RegisterEmailAndLog(emailAddress);
                await arg.CompleteMessageAsync(arg.Message);


            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}

