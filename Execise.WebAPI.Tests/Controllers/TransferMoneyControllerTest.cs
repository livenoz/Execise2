using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Migrations;
using Execise.WebAPI.Controllers;
using Execise.Model;
using Execise.Dtos;

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
            const int count = 10;
            const double balance1 = 100000;
            const double balance2 = 800000;
            const double amount1 = 30;
            const double amount2 = 20;
            const double totalBalance = (amount1 - amount2) * count;

            InitAccountBalance(fromAccountId, balance1);
            InitAccountBalance(toAccountId, balance2);
            //double fromAccountBalance;
            //double toAccountBalance;
            //GetAccountBalances(fromAccountId, toAccountId,
            //    out fromAccountBalance, out toAccountBalance);

            var controller = new TransferMoneyController();
            var result = new bool[count * 2];
            var tasks = new Task[count * 2];

            // Act
            for (int i = 0; i < count; i++)
            {
                var i1 = i;
                tasks[i] =
                    Task.Factory.StartNew(() =>
                        { result[i1] = controller.Post(new TransferModel {
                            FromAccountId = fromAccountId,
                            ToAccountId = toAccountId,
                            Amount = amount1
                        });
                    });

                var i2 = i + count;
                tasks[i2] =
                    Task.Factory.StartNew(() =>
                    { result[i2] = controller.Post(new TransferModel
                        {
                            FromAccountId = toAccountId,
                            ToAccountId = fromAccountId,
                            Amount = amount2
                        });
                    });
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
            Assert.AreEqual(balance1 - totalBalance, newFromAccountBalance);
            Assert.AreEqual(balance2 + totalBalance, newToAccountBalance);
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

        private void InitAccountBalance(int accountId, double balance)
        {
            using (var db = new MainModel())
            {
                var account = db.Accounts.Find(accountId);
                account.Balance = balance;
                db.Accounts.AddOrUpdate(account);
                db.SaveChanges();
            }
        }
    }
}
