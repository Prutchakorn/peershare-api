using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PeerShareV2.Data;
using PeerShareV2.Models;
using PeerShareV2.Providers;
using Microsoft.AspNetCore.Mvc;

namespace PeerShareV2.Controllers
{
    public class ApiController : BaseController
    {
        public ApiController(AWSProvider aWSProvider, ApplicationDbContext db, ISecurityProvider securityProvider) : base(db, securityProvider) { }
        
        public ActionResult Index()
        {
            return Json("It does work, What are you talking about? huh?!");
        }

        [HttpPost]
        public ActionResult PostBillSplit([FromBody]BillSplit billSplit)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _db.BillSplits.Add(billSplit);
                    _db.SaveChanges();
                    foreach(var p in billSplit.Peers)
                    {
                        var s = _db.UserAccounts.SingleOrDefault(x => x.PromptPay == p.PromptPay);
                        if(s != null)
                        {
                            p.UserAccountId = s.UserId;
                        }

                        p.BillSplitId = billSplit.Id;

                        var WithServiceCharge = p.PersonalTotalPrice + (billSplit.ServiceCharge * p.PersonalTotalPrice)/100;
                        p.PersonalNetPrice = WithServiceCharge + (billSplit.Vat * WithServiceCharge)/100;

                        p.StatusId = _statuses.SingleOrDefault(x => x.Name == "Pending").Id;
                        _db.Peers.Add(p);
                        _db.SaveChanges();
                        SendRTP(p);
                    }
                    
                    billSplit.CreatedDateTime = DateTime.Now;

                    _db.SaveChanges();
                    return Json(billSplit);
                }
                return Json("Model State is not valid");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(e.InnerException.Message);
            }
        }

        // [HttpPost]
        // public ActionResult Paid(long billSplitId, String payerPromptPay)
        // {
        //     var peer = _db.Peers.SingleOrDefault(x => x.BillSplitId == billSplitId &&
        //                                               x.PromptPay == payerPromptPay);
        //     if(peer != null)
        //     {
        //         try
        //         {
        //             peer.StatusId = _statuses.SingleOrDefault(x => x.Name == "Complete").Id;
        //             _db.SaveChanges();
        //             var statusDescription = _db.Statuses.SingleOrDefault(x => x.Id == peer.StatusId).Description;
        //             return Json("Status Changed to " + statusDescription);
        //         }
        //         catch(Exception e)
        //         {
        //             return Json(e.Message);
        //         }
        //     }
        //     else
        //     {
        //         return Json("Either Peer does not exist or PeerShare does not contian said Peer");
        //     }
        // }

        [HttpPost]
        public ActionResult GetAccount(string promptPay)
        {
            var user = _db.UserAccounts.SingleOrDefault(x => x.PromptPay == promptPay);
            return Json(user);
        }

        public ActionResult GetAllBillSplit(string promptPay)
        {
            var user = _db.UserAccounts.SingleOrDefault(x => x.PromptPay == promptPay);
            if(user != null)
            {
                List<BillSplit> userBillSplits = _db.BillSplits.Where(x => x.OwnerPromptPay == promptPay)
                                                               .OrderByDescending(x => x.CreatedDateTime)
                                                               .ToList();
                foreach (var uBS in userBillSplits)
                {
                    uBS.Peers = _db.Peers.Where(x => x.BillSplitId == uBS.Id).ToList();
                    foreach(var p in uBS.Peers)
                    {
                        p.Status = _db.Statuses.SingleOrDefault(x => x.Id == p.StatusId);
                    }
                }
                return Json(userBillSplits);
            }
            else
            {
                return Json("Promptpay not found");
            }
        }

        public ActionResult GetActiveBillSplit(string promptPay)
        {
            var user = _db.UserAccounts.SingleOrDefault(x => x.PromptPay == promptPay);
            if(user != null)
            {
                List<BillSplit> userBillSplits = _db.BillSplits.Where(x => x.OwnerPromptPay == promptPay &&
                                                                           x.IsActive)
                                                               .OrderByDescending(x => x.CreatedDateTime)
                                                               .ToList();
                foreach (var uBS in userBillSplits)
                {
                    uBS.Peers = _db.Peers.Where(x => x.BillSplitId == uBS.Id).ToList();
                    foreach(var p in uBS.Peers)
                    {
                        p.Status = _db.Statuses.SingleOrDefault(x => x.Id == p.StatusId);
                    }
                }
                
                return Json(userBillSplits);
            }
            else
            {
                return Json("Promptpay not found");
            }
        }

        public ActionResult GetInActiveBillSplit(string promptPay)
        {
            var user = _db.UserAccounts.SingleOrDefault(x => x.PromptPay == promptPay);
            if(user != null)
            {
                List<BillSplit> userBillSplits = _db.BillSplits.Where(x => x.OwnerPromptPay == promptPay &&
                                                                           !x.IsActive).ToList();
                foreach (var uBS in userBillSplits)
                {
                    uBS.Peers = _db.Peers.Where(x => x.BillSplitId == uBS.Id).ToList();
                    foreach(var p in uBS.Peers)
                    {
                        p.Status = _db.Statuses.SingleOrDefault(x => x.Id == p.StatusId);
                    }
                }
                return Json(userBillSplits);
            }
            else
            {
                return Json("Promptpay not found");
            }
        }

        [HttpPost]
        public ActionResult SendRTP([FromBody]Peer peer)
        {
            if(peer.IsPromptPay)
            {
                try
                {
                    var billSplit = _db.BillSplits.SingleOrDefault(x => x.Id == peer.BillSplitId);
                    var newRTP = new RTPModel{
                        SenderPromptPay = billSplit.OwnerPromptPay,
                        Amount = peer.PersonalNetPrice,
                        ReceiverPromptPay = peer.PromptPay,
                        RequestedDateTime = DateTime.Now,
                        IsActive = true
                    };
                    _db.RTPModels.Add(newRTP);
                    _db.SaveChanges();
                    var newPeerRTP = new PeerRTP{
                        PeerId = peer.Id,
                        RTPId = newRTP.Id
                    };

                    _db.PeerRTPs.Add(newPeerRTP);
                    _db.SaveChanges();
                    CountDownAlive(newRTP.Id);
                    return Json("Success");
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }
            }
            else
            {
                return Json("Peer doesn't use RTP");
            }
        }

        private async Task CountDownAlive(long RTPId)
        {
            var RTP = _db.RTPModels.SingleOrDefault(x => x.Id == RTPId);
            await Task.Run(async () =>
            {
                await Task.Delay(RTP.ActivePeriod * 1000 * 60 * 10);
                RTP.IsActive = false;
                _db.SaveChanges();
            });
        }

        public ActionResult BankNotification(string promptPay)
        {
            var activeRTP = _db.RTPModels.Where(x => x.IsActive &&
                                                     x.ReceiverPromptPay == promptPay)
                                         .OrderByDescending(x => x.RequestedDateTime)
                                         .ToList();
            if(activeRTP.Any())
            {
                return Json(activeRTP);
            }
            return Json("No Active RTP Found");
        }

        [HttpGet]
        public ActionResult BankAppResponse(long RTPId, Boolean IsPaid)
        {
            var RTP = _db.RTPModels.SingleOrDefault(x => x.Id == RTPId && x.IsActive);
            
            if(RTP != null)
            {
                var pId = _db.PeerRTPs.SingleOrDefault(x => x.RTPId == RTPId).PeerId;
                if(pId == 0)
                {
                    return Json("Peer not found");
                }
                var peer = _db.Peers.SingleOrDefault(x => x.Id == pId);
                
                if(IsPaid)
                {
                    peer.IsActive = false;
                    RTP.IsActive = false;
                    peer.StatusId = _statuses.SingleOrDefault(x => x.Name == "Complete").Id;
                    _db.SaveChanges();
                    CheckBillSplitStatus(peer.BillSplitId);
                    return Json("Payment Successful");
                }
                return Json("Cancelled");
            }
            return Json("RTP Notfound or time-out");
        }

        private void CheckBillSplitStatus(long BillSplitId)
        {
            var billSplit = _db.BillSplits.SingleOrDefault(x => x.Id == BillSplitId);
            if(billSplit != null)
            {
                var countP = _db.Peers.Count(x => x.BillSplitId == billSplit.Id &&
                                                  x.IsActive);
                if (countP == 0)
                {
                    billSplit.IsActive = false;
                    _db.SaveChanges();
                }                                                  
            }
        }
    }
}