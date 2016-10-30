using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Execise.Model;
using Execise.Service;

namespace Execise.WebAPI.Controllers
{
    public class TransferMoneyController: ApiController
    {

        public List<Account> GetAccounts()
        {
            return TransferMoney.GetAccounts();
        }

        public bool Post(int fromAccountId, int toAccountId, double balance)
        {
            return TransferMoney.Transfer(fromAccountId, toAccountId, balance);
        }
    }
}
