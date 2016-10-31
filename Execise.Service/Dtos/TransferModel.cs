using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Execise.Dtos
{
    public class TransferModel
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public double Amount { get; set; }
    }
}