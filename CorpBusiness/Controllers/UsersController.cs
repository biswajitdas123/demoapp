using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using CorpBusiness.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace CorpBusiness.Controllers
{
    public class UsersController : Controller
    {
         private readonly IDocumentSession _documentSession;
        public UsersController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        //
        // GET: /Users/

        //[ActionName("Users")]
        public ActionResult CreateUser(FormCollection collection)
        {

            return View();

        }

         [HttpPost]
        public ActionResult CreateUser(Mov move, FormCollection collection)
         {
             var log1 = _documentSession.Query<Mov>()
                .Where(x => x.Email == move.Email)
                .Take(1).ToList();

             if (log1.Count > 0)
             {
                 return View();
              
             }
             else
             {
                 if (!ModelState.IsValid)
                     return View();
                 _documentSession.Store(move);
                 _documentSession.SaveChanges();
                 Sendmail(move.Email, move.Name);
                
                 return RedirectToAction("Welcome", "Welcome", new { message = string.Format("Created user {0}", move.Email) });
             }  
         }

        public void Sendmail(string Email, string U_Id)
        {
            String Pass = GetUniqueKey();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("abc@gmail.com");
            msg.To.Add(Email);
            msg.Subject = "Registration successfull.";
            // msg.Body = "Your user Id :" + U_Id + " and Password :" + Pass;
            msg.Body = "Your registration is successfull.";
            msg.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            // smtp.Credentials = new NetworkCredential("email@gmail.com", "password");
            client.Credentials = new NetworkCredential("globalcoders@gmail.com", "coders12345");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
           // client.Send(msg);

        }
        public static string GetUniqueKey()
        {

            int maxSize = 8;
            char[] chars = new char[62];
            string a = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
    }
}
