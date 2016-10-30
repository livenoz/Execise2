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
            var db = new MainModel();
            var fromBalance = db.Accounts.Find(fromAccountId).Balance;
            var toBalance = db.Accounts.Find(toAccountId).Balance;
            var controller = new TransferMoneyController();
            var result = new bool[count * 2];
            var tasks = new Task[count * 2];

            // Act
            for (int i = 0; i < count; i++)
            {
                var i1 = i;
                tasks[i] =
                    Task.Factory.StartNew(() =>
                    { result[i1] = controller.Post(fromAccountId, toAccountId, 30); });

                var i2 = i + count;
                tasks[i2] =
                    Task.Factory.StartNew(() =>
                    { result[i2] = controller.Post(toAccountId, fromAccountId, 20); });
            }
            Task.WaitAll(tasks);

            // Assert
            var newDb = new MainModel();
            const int totalBalance = (30 - 20) * count;
            foreach (var item in result)
            {
                Assert.AreEqual(true, item);
            }
            Assert.AreEqual(fromBalance - totalBalance, newDb.Accounts.Find(fromAccountId).Balance);
            Assert.AreEqual(toBalance + totalBalance, newDb.Accounts.Find(toAccountId).Balance);
        }
    }
}
