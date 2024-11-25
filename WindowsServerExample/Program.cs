using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using static Quartz.Logging.OperationName;

namespace WindowsServerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] line = new string[] { $"from main - {args} " + DateTime.Now };
            File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
            List<IScheduler> lstScheduler = new List<IScheduler>();

            var companies = new List<string> { "CompanyA", "CompanyB", "CompanyC" };
            List<KeyValuePair<string, string>> lstKeyValuePair = new List<KeyValuePair<string, string>>()
                                                {
                                                    new KeyValuePair<string, string>("CompanyA", "ForQuartz"),
                                                    new KeyValuePair<string, string>("CompanyB", "ForQuartz1"),
                                                };
            foreach (var item in lstKeyValuePair)
            {
                NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type" , "json" },
                { "quartz.scheduler.instanceName", $"MyScheduler_{item.Value}" },
                { "quartz.scheduler.instanceId", "AUTO" },
                { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                { "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz" },
                { "quartz.jobStore.dataSource", $"{item.Value}" },
                { "quartz.jobStore.tablePrefix", "QRTZ_" },
                { $"quartz.dataSource.{item.Value}.connectionString", $"Server=DESKTOP-J6SDVUR;Database={item.Value};User Id=sa;Password=focus" },
                { $"quartz.dataSource.{item.Value}.provider", "SqlServer" },
                { "quartz.jobStore.clustered", "false" }
            };
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory(props);
                IScheduler Scheduler = schedulerFactory.GetScheduler().Result;//.Result;
                Scheduler.UnscheduleJob(new TriggerKey("sampleJob", "defaultGroup"));
                Scheduler.DeleteJob(new JobKey("sampleJob", "defaultGroup"));
                Scheduler.UnscheduleJob(new TriggerKey("sampleJob2", "defaultGroup2"));
                Scheduler.DeleteJob(new JobKey("sampleJob2", "defaultGroup2"));

                // Define the job and tie it to the SampleJob class
                IJobDetail job = JobBuilder.Create<backgroundJobs>()
                     .WithIdentity($"sampleJob_{item.Value}", $"defaultGroup_{item.Value}")
                     .Build();

                // Create a trigger to run every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"sampleJob_{item.Value}", $"defaultGroup_{item.Value}")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(100).RepeatForever())
                    .Build();

                var jobkey = Scheduler.CheckExists(job.Key);
                if (jobkey != null && !jobkey.Result)
                {
                    Scheduler.ScheduleJob(job, trigger);
                }

                IJobDetail job2 = JobBuilder.Create<backgroundJobs>()
               .WithIdentity($"sampleJob2_{item.Value}", $"defaultGroup2_{item.Value}")
               .Build();

                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity($"sampleJob2_{item.Value}", $"defaultGroup2_{item.Value}")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(180).RepeatForever())
                    .Build();
                var jobkey1 = Scheduler.CheckExists(job2.Key);
                if (jobkey1 != null && !jobkey1.Result)
                {
                    Scheduler.ScheduleJob(job2, trigger2);
                }
                lstScheduler.Add(Scheduler);                                                       //Scheduler.Start();
            }

            //foreach (var item in companies)
            {
                HostFactory.Run(hostconfigration =>
                {
                    hostconfigration.Service<BackgroudTimer>(s =>
                    {
                        s.ConstructUsing(backgroudTimer => new BackgroudTimer());
                        s.WhenStarted(backgroudTimer => backgroudTimer.Start(lstScheduler));
                        s.WhenStopped(backgroudTimer => backgroudTimer.Stop());
                    });
                    hostconfigration.RunAsLocalSystem();
                    hostconfigration.StartAutomatically();

                    hostconfigration.SetServiceName($"BackGroudservices");
                    hostconfigration.SetDisplayName($"Back Groud services");
                    hostconfigration.SetDescription($"to run background tasks");
                });
            }
            //HostFactory.Run(hostconfigration => {
            //    hostconfigration.Service<BackgroudTimer>(s => {
            //        s.ConstructUsing(backgroudTimer => new BackgroudTimer());
            //        s.WhenStarted(backgroudTimer => backgroudTimer.Start());
            //        s.WhenStopped(backgroudTimer => backgroudTimer.Stop());
            //    });
            //    hostconfigration.RunAsLocalSystem();
            //    hostconfigration.SetServiceName("BackGroudservices");
            //    hostconfigration.SetDisplayName("Back Groud services");
            //    hostconfigration.SetDescription("to run background tasks");
            //});

        }
    }
}
