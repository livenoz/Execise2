using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Execise.Model;
using Execise.WebAPI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Execise.WebAPI.Tests.Controllers
{
    [TestClass]
    public class TransferMoneyControllerTest
    {
        [TestMethod]
        public void Post()
        {
            // Arrange
            const int fromAccountId = 1;
            const int toAccountId = 2;
            const int count = 500;
            const double amount1 = 30;
            const double amount2 = 20;
            const double totalBalance = (amount1 - amount2) * count;

            double fromAccountBalance;
            double toAccountBalance;
            GetAccountBalances(fromAccountId, toAccountId,
                out fromAccountBalance, out toAccountBalance);

            if (fromAccountBalance >= amount1 * count
                && toAccountBalance >= amount2 * count)
            {
                var controller = new TransferMoneyController();
                var result = new bool[count * 2];
                var tasks = new Task[count * 2];

                // Act
                for (int i = 0; i < count; i++)
                {
                    var i1 = i;
                    tasks[i] =
                        Task.Factory.StartNew(() =>
                        { result[i1] = controller.Post(fromAccountId, toAccountId, amount1); });

                    var i2 = i + count;
                    tasks[i2] =
                        Task.Factory.StartNew(() =>
                        { result[i2] = controller.Post(toAccountId, fromAccountId, amount2); });
                }
                Task.WaitAll(tasks);

                // Assert
                double newFromAccountBalance;
                double newToAccountBalance;
                GetAccountBalances(fromAccountId, toAccountId,
                    out newFromAccountBalance, out newToAccountBalance);

                foreach (var item in result)
                {
                    Assert.AreEqual(true, item);
                }
                Assert.AreEqual(fromAccountBalance - totalBalance, newFromAccountBalance);
                Assert.AreEqual(toAccountBalance + totalBalance, newToAccountBalance);
            }
        }

        private void GetAccountBalances(int fromAccountId, int toAccountId,
            out double fromAccountBalance, out double toAccountBalance)
        {
            using (var db = new MainModel())
            {
                fromAccountBalance = db.Accounts.Find(fromAccountId).Balance;
                toAccountBalance = db.Accounts.Find(toAccountId).Balance;
            }
        }
    }
}
