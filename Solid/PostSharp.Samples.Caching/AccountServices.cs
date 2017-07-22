using System;
using System.Collections.Generic;
using System.Threading;
using PostSharp.Patterns.Caching;

namespace PostSharp.Samples.Caching
{
    [CacheConfiguration( ProfileName = "Account" )]
    class AccountServices
    {
        [Cache]
        public static Account GetAccount(int id)
        {
            Console.WriteLine($">> Retrieving the account {id} from database...");
            Thread.Sleep(1000);

            var account = new Account { AccountId = id };

            CachingServices.CurrentContext.AddDependency(account);

            return account;
        }

        [Cache]
        public static IEnumerable<Account> GetAccountsOfCustomer(int customerId)
        {
            yield return GetAccount(1);
            yield return GetAccount(2);
        }

        public static void UpdateAccount(Account account)
        {
            Console.WriteLine($">> Updating the account {account.AccountId} in database...");
            Thread.Sleep(1000);
            CachingServices.Invalidation.Invalidate(account);
        }
    }
}