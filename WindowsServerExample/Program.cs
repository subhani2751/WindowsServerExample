using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace WindowsServerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostconfigration => {
                hostconfigration.Service<BackgroudTimer>(s => {
                    s.ConstructUsing(backgroudTimer => new BackgroudTimer());
                    s.WhenStarted(backgroudTimer => backgroudTimer.Start());
                    s.WhenStopped(backgroudTimer => backgroudTimer.Stop());
                });
                hostconfigration.RunAsLocalSystem();
                hostconfigration.SetServiceName("BackGroudservices");
                hostconfigration.SetDisplayName("Back Groud services");
                hostconfigration.SetDescription("to run background tasks");
            });

        }
    }
}
