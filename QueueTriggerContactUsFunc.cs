using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace Symphony.Functions
{
    public class QueueTriggerContactUsFunc
    {
        [FunctionName("QueueTriggerContactUsFunc")]
        public void Run(
            [QueueTrigger("contactus", Connection = "AzureWebJobsStorage")]ContactUs contactUs, 
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
            ILogger log)
        {
            var email = new EmailAddress(Environment.GetEnvironmentVariable("EmailTo"));

            message = new SendGridMessage();
            message.From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender"));
            message.AddTo(email);
            message.Subject = "Contact Us Message";
            message.HtmlContent = $"<p>User: {contactUs.User}</p> <p>Email: {contactUs.Email}</p> <p>Message: {contactUs.Message}</p>";
            
            log.LogInformation($"C# Queue trigger function processed: User: {contactUs.User} Email: {contactUs.Email} Message: {contactUs.Message}");
        }
    }
}
