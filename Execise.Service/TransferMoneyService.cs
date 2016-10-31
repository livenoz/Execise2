using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using Execise.Model;
using Execise.Dtos;

namespace Execise.Service
{
    public class TransferMoneyService
    {
        private static Dictionary<int, String> _dictionaryLock = new Dictionary<int, String>();
        public static bool Transfer(TransferModel model)
        {
            var result = false;
            if (Validation(model))
            {
                AddAccountToDictionary(model.FromAccountId);
                AddAccountToDictionary(model.ToAccountId);

                var orderIds = new[] { model.FromAccountId, model.ToAccountId }
                                .OrderBy(c => c).ToArray();
                lock (_dictionaryLock[orderIds[0]])
                {
                    lock (_dictionaryLock[orderIds[1]])
                    {
                        //Thread.Sleep(5000);
                        Thread.Sleep(100);
                        using (var db = new MainModel())
                        {
                            var fromAccount = db.Accounts.Find(model.FromAccountId);
                            if (fromAccount.Balance >= model.Amount)
                            {
                                var toAccount = db.Accounts.Find(model.ToAccountId);
                                fromAccount.Balance -= model.Amount;
                                toAccount.Balance += model.Amount;
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

        private static bool Validation(TransferModel model)
        {
            var result = false;
            if (model.Amount >= 0)
            {
                using (var db = new MainModel())
                {
                    if (db.Accounts.Find(model.FromAccountId) != null
                        && db.Accounts.Find(model.ToAccountId) != null)
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
