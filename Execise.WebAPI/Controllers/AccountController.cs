using System.Collections.Generic;
using System.Web.Http;
using Execise.Model;
using Execise.Service;
using Execise.Dtos;

namespace Execise.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        public List<Account> Get()
        {
            return TransferMoneyService.GetAccounts();
        }
    }
}
