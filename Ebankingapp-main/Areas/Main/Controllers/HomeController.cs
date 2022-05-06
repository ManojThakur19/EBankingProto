using DbHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MigrationHelper.Models;
using MigrationHelper.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EbankingApp.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        private readonly IDataHelper _Helper;
        private readonly IHttpContextAccessor _httpContAccessor;
        private IHttpClientFactory httpClientFactory;

        public HomeController(IDataHelper helper, IHttpContextAccessor httpContAccessor, IHttpClientFactory httpClientFactory)
        {
            _Helper = helper;
            _httpContAccessor = httpContAccessor;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = httpClientFactory.CreateClient("API Client");
            var result = await client.GetAsync("http://api.exchangeratesapi.io/v1/latest?access_key=14fff920674f1a2697abf815c52c903a&format=1");
            var content = await result.Content.ReadAsStringAsync();

            var val  = JsonConvert.DeserializeObject<ExchangeRate>(content);

            var data = await _Helper.GetRate();
            
            await _Helper.DeleteRateById(data.FirstOrDefault().ID);
            await _Helper.AddRate
               (
                 val.rates.EUR,
                 val.rates.GBP,
                 val.rates.USD
               );

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));

            var userAccountData = await _Helper.GetUserAccountByIdAsync(data.ID);
            var rateData = await _Helper.GetRate();


            //use api to convert currecny
            double balance = 0;
            foreach (var item in userAccountData)
            {
                if(item.CurrencyId == 1) 
                {
                    balance = balance + item.Balance;
                }
                else if (item.CurrencyId == 2) 
                {
                    balance = balance + item.Balance * rateData.FirstOrDefault().GBP;
                }
                else 
                {
                    balance = balance + item.Balance * 
                        (rateData.FirstOrDefault().GBP/ rateData.FirstOrDefault().USD);
                }
            }

            //need to change balance after transaction

            var model = new ProfileInfo()
            {
                FullName = data.Firstname + " " + data.Lastname,
                AccNum = data.AccNum,
                AccountType = data.AccountType,
                Balance = Convert.ToString(balance)
            };

            return View(model);
        }

        public async Task<IActionResult> LoadFund()
        {
            var currencyData = await   _Helper.GetCurrency();
           ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadFund(UserAccount model)
        {
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));
            
            //save to user accounts
            await _Helper.LoadFund
                (
                data.ID,
                data.AccNum,
                model.Balance,
                model.CurrencyId,
                DateTime.Now,
                model.Remark
                );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Transactionsum()
        {
            try
            {
                var userId = Convert.ToInt32(_httpContAccessor.HttpContext.User.FindFirstValue("Id"));

                var transactions = (await _Helper.GetTransaction()).Where(x => x.FromUserId == userId || x.ToUserId == userId).OrderByDescending(x => x.Date.Date);

                var fromUserIds = transactions.Select(x => x.FromUserId).ToList();
                var toUserIds = transactions.Select(x => x.ToUserId).ToList();

                var distinctUserIds = fromUserIds.Concat(toUserIds).Distinct().ToList();

                List<Register> users = new List<Register>();
                foreach (var distId in distinctUserIds)
                {
                    users.Add(await _Helper.GetUserByIdAsync(distId));
                }

                
                var distinctCurrencyIds = transactions.Select(x => x.CurrencyId).Distinct();
                var currencies = (await _Helper.GetCurrency()).Where(x => distinctCurrencyIds.Contains(x.ID));

                List<TransactionSummaryViewModel> datas = new List<TransactionSummaryViewModel>();
                foreach (var transaction in transactions)
                {
                    var data = new TransactionSummaryViewModel
                    {
                        Id = transaction.ID,
                        Amount = transaction.Amount,
                        CurrencyId = transaction.CurrencyId,
                        CurrencyName = currencies.First(x=>x.ID == transaction.CurrencyId).name,
                        FromAccount = transaction.FromAccNum,
                        FromUserId = transaction.FromUserId,
                        FromFullName = users.Find(x => x.ID == transaction.FromUserId).Firstname,
                        FromAccountType = users.Find(x => x.ID == transaction.FromUserId).AccountType?.ToString(),
                        ToAccount = transaction.ToAccNum,
                        ToUserId = transaction.ToUserId,
                        ToFullName = users.Find(x => x.ID == transaction.ToUserId).Firstname,
                        ToAccountType = users.Find(x => x.ID == transaction.ToUserId).AccountType?.ToString(),
                        Remarks = transaction.Remarks,
                        CreatedDate = transaction.Date,
                    };
                    datas.Add(data);
                }

                return View(datas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Transaction()
        {
            var currencyData = await _Helper.GetCurrency();
            ViewData["CurrencyData"] = new SelectList(currencyData, "ID", "name");

            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));

            var userAccountData = await _Helper.GetUserAccountByIdAsync(data.ID);
            var rateData = await _Helper.GetRate();

            double balance = 0;
            foreach (var item in userAccountData)
            {
                if (item.CurrencyId == 1)
                {
                    balance = balance + item.Balance;
                }
                else if (item.CurrencyId == 2)
                {
                    balance = balance + item.Balance * rateData.FirstOrDefault().GBP;
                }
                else
                {
                    balance = balance + item.Balance *
                        (rateData.FirstOrDefault().GBP / rateData.FirstOrDefault().USD);
                }
            }

            var transData = new TransactionViewModel();

            transData.FromAccNum = data.AccNum;
            transData.FromUserId = data.ID;
            transData.FromName = data.Firstname +" "+data.Lastname;
            transData.Balance = balance;

            return View(transData);
        }

        [HttpPost]
        public async Task<IActionResult> Transaction(TransactionViewModel model)
        {
            var rateData = await _Helper.GetRate();
            bool check = false;
            string message = "";
            if (model.CurrencyId == 1) 
            {
                if (model.Amount > model.Balance)
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }
            else if(model.CurrencyId == 2) 
            {
                if(model.Amount > model.Balance * rateData.FirstOrDefault().GBP)
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }
            else 
            {
                if (model.Amount > model.Balance * 
                        (rateData.FirstOrDefault().GBP / rateData.FirstOrDefault().USD))
                {
                    message = "Not Enough Balance";
                    check = true;
                }
            }

            var chk = await _Helper.GetUserByAccountNumberAsync(model.ToAccNum);
            var userId = _httpContAccessor.HttpContext.User.FindFirstValue("Id");
            var data = await _Helper.GetUserByIdAsync(Convert.ToInt32(userId));
            if (chk != null)
            {

                await _Helper.AddTransaction(Convert.ToInt32(userId), chk.ID,
                                     data.AccNum, model.ToAccNum,
                                     model.Amount, model.Remarks,
                                     model.CurrencyId
                                );
            }
            else 
            {
                check = true;
                message = "Given Account is not available";
            }
            ViewBag.Result = message;
            return RedirectToAction("Transaction");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
