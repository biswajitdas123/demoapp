using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Newtonsoft.Json;
using CorpBusiness.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace CorpBusiness.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private readonly IDocumentSession _documentSession;
        public AdminController(IDocumentSession documentSession)
        {
             _documentSession = documentSession;
            
        }
        [ActionName("Login")]
        public ActionResult Login()
        {
            var model = new LoginPage();
            return View(model);
        }

        [ActionName("Business")]
        public ActionResult BusinessList(String message)
        {
            if (Session["user"] != null)
            {
                var mov = _documentSession.Query<Vendor>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(mov);
            }
            else
            {
                // return View(adcl);
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }



        }
        public ActionResult CreateBusiness(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult CreateBusiness(Vendor vndr)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    return View();
                _documentSession.Store(vndr);
                _documentSession.SaveChanges();
                return RedirectToAction("Business", new { message = string.Format("Business Added Successfully", vndr.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        public ActionResult EditBusiness(int id)
        {
            if (Session["user"] != null)
            {
                var bsn = _documentSession.Load<Vendor>(id);
                if (bsn == null)
                    return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });

                return View(bsn);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult EditBusiness(Vendor bsn)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                    _documentSession.Store(bsn);
                _documentSession.SaveChanges();
                return RedirectToAction("Business", new { message = string.Format("Saved changes to Business {0}", bsn.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [ActionName("DetailsBusiness")]
        public ActionResult DetailsBusiness(int id)
        {
            if (Session["user"] != null)
            {
                var bsn = _documentSession.Load<Vendor>(id);
                if (bsn == null)
                    return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });
                return View(bsn);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
         public ActionResult DeleteBusiness(int id)
         {
             if (Session["user"] != null)
             {
                 var move = _documentSession.Load<Vendor>(id);
                 if (move == null)
                     return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });
                 return View(move);
             }
             else
             {
                 return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
             }
         }

         //
         // POST: /Vendor/Delete/5

         [HttpPost]
         public ActionResult DeleteBusiness(int id, FormCollection collection)
         {

             if (Session["user"] != null)
             {
                 _documentSession.Delete(_documentSession.Load<Vendor>(id));
                 _documentSession.SaveChanges();
                 return RedirectToAction("Business", new { message = string.Format("Delete mov with the id {0}", id) });
             }
             else
             {
                 return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
             }
            
         }
        
       
        /// <summary>
        /// /////////////////////////////////////////////////////Category/////////////
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        [ActionName("Category")]
        public ActionResult CategoryList(String message)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Query<Category>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [HttpPost]
        public ActionResult Login(AdminLoginPage admn)
        {
            
            if (admn.UserID == "Admin" && admn.Password == "admin")
            {
                Session["user"] = "Admin";
               // Session["user1"] = "us";
                return RedirectToAction("Users", "Admin", new { message = string.Format("Logged In As {0}", admn.UserID) });
            }
            else
            {
                return View();
            }
        }
        [ActionName("LogOff")]
        public ActionResult LogOff()
        {
            var model = new LoginPage();
            Session.Abandon();
            if (Session["user"] != null || Session["user1"] !=null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            
        }

        //[ActionName("DetailsCat")]
        public ActionResult DetailsCat(int id)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Load<Category>(id);
                if (cat == null)
                    return RedirectToAction("Category", new { message = string.Format("cat {0} not found", id) });
                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // GET: /Admin/Create

        public ActionResult CreateCat(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        } 

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult CreateCat(Category cat)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    return View();
                _documentSession.Store(cat);
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Category Successfully Created", cat.Categories) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        
        //
        // GET: /Admin/Edit/5
 
        public ActionResult EditCat(int id)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Load<Category>(id);
                if (cat == null)
                    return RedirectToAction("Category", new { message = string.Format("cat {0} not found", id) });

                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult EditCat(Category cat)
        {
            if (Session["user"] != null)
            {
                if (ModelState.IsValid)
                    _documentSession.Store(cat);
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Saved changes to cat {0}", cat.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // GET: /Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteCat(int id, FormCollection collection)
        {
            if (Session["user"] != null)
            {
                _documentSession.Delete(_documentSession.Load<Category>(id));
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            //if(Session["user"]!=null)
            //{
            //    // TODO: Add delete logic here
               

            //    return RedirectToAction("Category");
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            //}
        }

        //
        // POST: /Admin/Delete/5

        
        public ActionResult DeleteCat(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Category>(id);
                if (move == null)
                    return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            
            //if (Session["user"] != null)
            //{
            //    _documentSession.Delete(_documentSession.Load<Category>(id));
            //    _documentSession.SaveChanges();
            //    return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
            //}
            //else
            //{
            //    return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            //}
        }



        /////////////////////////////////////////////////User//////////////////////////


        [ActionName("Users")]
        public ActionResult UserList(String message)
        {
            if (Session["user"] != null)
            {
                var mov = _documentSession.Query<Mov>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(mov);
            }
            else
            {
                // return View(adcl);
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }



        }
        public ActionResult DetailsUser(int id)
        {
            //var mov=_documentSession.Query<>;
            if (Session["user"] != null)
            {
                var mov = _documentSession.Load<Mov>(id);
                if (mov == null)
                    return RedirectToAction("Users", new { message = string.Format("mov {0} not found", id) });
                return View(mov);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        public ActionResult CreateUser(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }

        }

        [HttpPost]
        public ActionResult CreateUser(Mov move)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    return View();
                _documentSession.Store(move);
                _documentSession.SaveChanges();
                Sendmail(move.Email, move.Name);
                return RedirectToAction("Users", new { message = string.Format("Created User {0}", move.Email) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        public ActionResult EditUser(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Mov>(id);
                if (move == null)
                    return RedirectToAction("Users", new { message = string.Format("Mov {0} not found", id) });

                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [HttpPost]
        public ActionResult EditUser(Mov move)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    _documentSession.Store(move);
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Saved changes to mov {0}", move.Id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }


        }

        [ActionName("DeleteUser")]
        public ActionResult ConfirmDelete(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Mov>(id);
                if (move == null)
                    return RedirectToAction("Users", new { message = string.Format("move {0} not found", id) });
                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            if (Session["user"] != null)
            {
                _documentSession.Delete(_documentSession.Load<Mov>(id));
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Delete mov with the id {0}", id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
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
            client.Send(msg);

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

        ///////////////////////////////////Change Password/////////////////////////////

        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(int id)
        {


            var move = _documentSession.Load<Mov>(id);
            if (move == null)
                return RedirectToAction("ChangePassword", new { message = string.Format("Password Change For{0}", move.Id) });

            return View(move);


        }
        [HttpPost]
        public ActionResult ChangePassword(Mov move,int id)
        {

           // var log2 = _documentSession.Load<ChangePassword>(chng.UserId);
            var log1 = _documentSession.Query<Mov>()
                .Where(x => x.Email == move.Email && x.Password == Request["Name0"])
                .Take(1).ToList();
          
            if (log1.Count > 0)
            {
                if (!ModelState.IsValid)

                    move.Password = "";
                    move.Id = 0;
                    move.Password = Request["Name1"];
                    move.RetypePassword = Request["Name2"];

                _documentSession.Delete(_documentSession.Load<Mov>(id));
                _documentSession.SaveChanges();
                _documentSession.Store(move);
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Password Successfully Changed")});
            }

            else
            {
                // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
                return View(move);
            }

        }
        
    }
}
