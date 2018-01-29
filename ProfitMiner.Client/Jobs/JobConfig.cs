using System;
using Hangfire;
using Hangfire.MemoryStorage;

namespace ProfitMiner.Client.Jobs
{
    class JobConfig
    {
        public static void Configue()
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();

            RecurringJob.AddOrUpdate(() => WalletLogginJob.Execute(), Cron.MinuteInterval(5), TimeZoneInfo.Local);
        }
    }
}