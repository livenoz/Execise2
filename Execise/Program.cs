using System.Net.Http.Headers;
using Execise.Service;
using Execise.Dtos;

namespace Execise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TransferMoneyService.Transfer(new TransferModel
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 1000
            });
        }
    }
}
