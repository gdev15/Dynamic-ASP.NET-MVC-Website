﻿using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace PortfolioWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.ActiveLink = "Home";
            return View();
        }

        public IActionResult Resume()
        {
            ViewBag.ActiveLink = "Resume";
            return View();
        }

        public IActionResult Portfolio()
        {
            ViewBag.ActiveLink = "Portfolio";
            return View();
        }

        public IActionResult Links()
        {
            ViewBag.ActiveLink = "Links";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.ActiveLink = "Contact";
            return View();
        }
        //Renamed 404.html page to ErrorPage
        public IActionResult ErrorPage(){

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Contact Action for GET Request
        [HttpPost] //Denotes the action will handle post request
        //This overload method should take the argument of the contact model
        public IActionResult Contact(ContactViewModel cvm)
        {
            //Condition to check if the model has data in it
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            //Format for the email
            string message = $"You have received a new email from your site's contact form! <br/>" + $"Sender: {cvm.Name}<br/>Email: {cvm.Email} <br/>{cvm.Subject}<br />Message: {cvm.Message}";

            //MimeMessage object to assist with storing and transporting the email
            var mm = new MimeMessage();

            //Using the add method to pass in a a new object of type MailboxAddress
            //With the name of the Sender and the address which will be retrieved
            //using the GetValue property to retrieve the data that is stored in the JSON object
            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            //Nothing to retrieve from the JSON object
            //So just need to save the users input 
            mm.Subject = cvm.Subject;

            //Body of the email to be formatted in html using the message variable that 
            //was created above
            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;

            //Collects the users email that filled out the form
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));


            using (var client = new SmtpClient())
            {
                //connect to the mail server
                client.Connect(_config.GetValue<string>(
                    "Credentials:Email:Client"), 8889);

                client.Authenticate(
                        //Username
                        _config.GetValue<string>(
                            "Credentials:Email:User"),
                        //Password
                        _config.GetValue<string>("Credentials:Email:Password")
                        
                        );                
                try
                {
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please try again later. <br/>" + $"Error Message: {ex.StackTrace}";

                    return View(cvm);
                }

            }

            return View("EmailConfirmation", cvm);

        }

    }
}