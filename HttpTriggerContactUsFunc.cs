using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Symphony.Functions
{
    public static class HttpTriggerContactUsFunc
    {
        [FunctionName("HttpTriggerContactUsFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Queue("contactus")] IAsyncCollector<ContactUs> contactUsQueue,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var contactUs = JsonConvert.DeserializeObject<ContactUs>(requestBody);

            await contactUsQueue.AddAsync(contactUs);

            log.LogInformation($"User: {contactUs.User} Email: {contactUs.Email} Message: {contactUs.Message}");
            return new OkObjectResult($"This HTTP triggered function executed successfully.");
        }
    }

    public class ContactUs 
    {
        public string User { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
