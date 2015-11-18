using System;
using System.ServiceProcess;

namespace pdfforge.PrinterHelper
{
    public class Service
    {
        public static Error RestartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var millisec1 = Environment.TickCount;
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                var millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return null;
            }
            catch
            {
                return new Error(Error.ERROR_INTERNAL, Messages.RestartSpoolerServiceFailed);
            }
        }
    }
}
