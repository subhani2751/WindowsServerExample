using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServerExample
{
    public class backgroundJobs : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //Console.WriteLine($"SampleJob executed at {DateTime.Now}");
                string[] line = new string[] { context.JobDetail.Key + " - " + DateTime.Now };
                File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);

            }
            catch (Exception ex)
            {
                string[] line = new string[] { "error - " + DateTime.Now };
                File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
            }
            return Task.CompletedTask;
        }
    }
}
