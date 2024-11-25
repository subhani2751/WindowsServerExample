using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf.Builders;

namespace WindowsServerExample
{
    public class BackgroudTimer
    {
        private readonly Timer _timer;
        //public BackgroudTimer()
        //{
        //    _timer = new Timer(50000) { AutoReset = true };
        //    _timer.Elapsed += timerElapsed;

        //}
        //private void timerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    string[] line = new string[] { DateTime.Now.ToString() };
        //    File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
        //}
        public void Start(List<IScheduler> lstScheduler = null) 
        {
            string[] line = new string[] { "starting..." };
            //_timer.Start();
            File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
            foreach (IScheduler item in lstScheduler)
            {
                item.Start();
            }
        }
        public void Stop() 
        {
            string[] line = new string[] { "stoping..." };
            File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
            //_timer.Stop();
        }
    }
}
