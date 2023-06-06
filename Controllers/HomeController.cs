using System.Diagnostics;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PersonalSiteFixed.Models;

namespace PersonalSiteFixed.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Links()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult DungeonDetails()
        {
            return View();
        }

        public IActionResult StoreFrontDetails()
        {
            return View();
        }

        public IActionResult ReactJS()
        {
            return View();
        }

        public IActionResult Hogwarts()
        {
            return View();
        }

        public IActionResult Capstone()
        {
            return View();
        }

        public IActionResult PersonalSiteReact()
        {
            return View();
        }

        //Add a field for the Configuration settings in appsettings.json
        private readonly IConfiguration _config;

        //Add a constructor for our controller that accepts the config info as a parameter
        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string message = $"You have received a new email from your site's contact form!<br />" +
                $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />" +
                $"Message: {cvm.Message}";

            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            using (var client = new SmtpClient())
            {
                //Connect to the mail server using the credentials in our appsettings.json
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                //Login to the mail server using the credentials for our email user
                client.Authenticate(

                    //Username
                    _config.GetValue<string>("Credentials:Email:User"),

                    //Password
                    _config.GetValue<string>("Credentials:Email:Password")

                    );

                //It's possible the mail server may be down when the user attempts to contact us.
                //so we can encapsulate our code to send the message in a try/catch.

                try
                {
                    //Try to send the email
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    //If there is an issue, we can store an error message in a ViewBag variable
                    //to be displayed in the view
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please " +
                        $"try again later.<br />Error Message: {ex.StackTrace}";

                    //Return the user to the View with their form info intact
                    return View(cvm);

                }

            }

            //If all goes well, return a View that displays a confirmation to the user
            //that their email was sent.

            return View("EmailConfirmation", cvm);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}