using System.Collections.Generic;
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
