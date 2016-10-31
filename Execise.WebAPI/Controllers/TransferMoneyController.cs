using System.Collections.Generic;
using System.Web.Http;
using Execise.Model;
using Execise.Service;
using Execise.Dtos;

namespace Execise.WebAPI.Controllers
{
    public class TransferMoneyController: ApiController
    {
        public bool Post(TransferModel model)
        {
            return TransferMoneyService.Transfer(model);
        }
    }
}
