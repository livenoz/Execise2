using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using Execise.Model;

namespace Execise.Service
{
    public class TransferMoney
    {
        private static Dictionary<int, String> _dictionaryLock = new Dictionary<int, String>();
        public static bool Transfer(int fromAccountId, int toAccountId, double balance)
        {
            var result = false;
            if (Validation(fromAccountId, toAccountId, balance))
            {
                AddAccountToDictionary(fromAccountId);
                AddAccountToDictionary(toAccountId);

                var orderIds = new[] { fromAccountId, toAccountId }.OrderBy(c => c).ToArray();
                lock (_dictionaryLock[orderIds[0]])
                {
                    lock (_dictionaryLock[orderIds[1]])
                    {
                        //Thread.Sleep(5000);
                        using (var db = new MainModel())
                        {
                            var fromAccount = db.Accounts.Find(fromAccountId);
                            if (fromAccount.Balance >= balance)
                            {
                                var toAccount = db.Accounts.Find(toAccountId);
                                fromAccount.Balance -= balance;
                                toAccount.Balance += balance;
                                db.Accounts.AddOrUpdate(fromAccount);
                                db.Accounts.AddOrUpdate(toAccount);
                                db.SaveChanges();

                                result = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static void AddAccountToDictionary(int accountId)
        {
            if (!_dictionaryLock.ContainsKey(accountId))
            {
                //Todo:
                lock (_dictionaryLock)
                {
                    if (!_dictionaryLock.ContainsKey(accountId))
                    {
                        using (var db = new MainModel())
                        {
                            var account = db.Accounts.Find(accountId);
                            _dictionaryLock.Add(account.Id, account.Number);
                        }
                    }
                }
            }
        }

        private static bool Validation(int fromAccountId, int toAccountId, double balance)
        {
            var result = false;
            if (balance >= 0)
            {
                using (var db = new MainModel())
                {
                    if (db.Accounts.Find(fromAccountId) != null
                        && db.Accounts.Find(toAccountId) != null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public static List<Account> GetAccounts()
        {
            using (var db = new MainModel())
            {
                return db.Accounts.ToList();
            }
        }

    }
}
