using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PeerShareV2.Data;
using PeerShareV2.Models;
using PeerShareV2.Providers;

namespace PeerShareV2.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(AWSProvider aWSProvider,
                                 ApplicationDbContext db,
                                 ISecurityProvider securityProvider) : base(db, securityProvider) { }
        
        [HttpPost]
        public ActionResult Register([FromBody]UserAccount userAccount)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    userAccount.Password = _securityProvider.GetEncrypt(userAccount.Password);
                    _db.UserAccounts.Add(userAccount);
                    _db.SaveChanges();
                    return Json(true);
                }
                return Json("Model State is Invalid");
            }   
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(e.InnerException.Message);
            }
        }

        [HttpPost]
        public ActionResult LogIn([FromBody]LogInModel logInModel)
        {
            var user = _db.UserAccounts.Where(x => x.PromptPay == logInModel.PromptPay &&
                                               x.Password == _securityProvider.GetEncrypt(logInModel.Password))
                                   .SingleOrDefault();
            return Json(user != null);
        }
    }   
}