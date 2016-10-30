using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Execise.Service;

namespace Execise.WebAPI.Controllers
{
    public class TransferMoneyController: ApiController
    {
        [HttpGet]
        [HttpPost]
        public bool Post(int fromAccountId, int toAccountId, double balance)
        {
            return TransferMoney.Transfer(fromAccountId, toAccountId, balance);
        }
    }
}
