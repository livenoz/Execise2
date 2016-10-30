using System.Net.Http.Headers;
using Execise.Service;

namespace Execise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TransferMoney.Transfer(1, 2, 10000);
        }
    }
}
