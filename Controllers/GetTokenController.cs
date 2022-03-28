﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetTokenController : ControllerBase
    {
        public static string serverToken;

        [HttpGet]
        public StatusCodeResult Get(string email)
        {
            try
            {
                serverToken = BCrypt.Net.BCrypt.HashPassword($"{DateTime.Now.ToString()}{DateTime.Now.Millisecond.ToString()}");
                SendEmail(email,serverToken);
            }
            catch (Exception)
            {

                return StatusCode(204);
            }
           
            return StatusCode(200);
        }

        public static bool CheckToken(string token)
        {
           
            if (serverToken!=null &&serverToken == token )
                return true;
            else
                return false;
        }

        public static void SendEmail(string email,string hash)
        {
            var fromAddress = new MailAddress("vovkaprikhod@gmail.com", "Automatic Email");
            var toAddress = new MailAddress($"{email}", "To Name");
            const string fromPassword = "Prikhod322";
            const string subject = "Your Token";
            string body = $"{hash}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.EnableSsl = true;
                smtp.Send(message);
            }

        }


    }
}
