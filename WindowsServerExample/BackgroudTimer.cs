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
        public BackgroudTimer()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += timerElapsed;

        }
        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] line = new string[] { DateTime.Now.ToString() };
            File.AppendAllLines(@"C:\Windows\Temp\BackgroudTimer.txt", line);
        }
        public void Start() 
        {
            _timer.Start();
        }
        public void Stop() 
        { 
            _timer.Stop();
        }
    }
}
