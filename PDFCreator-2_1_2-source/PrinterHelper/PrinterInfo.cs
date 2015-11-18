using System.Text;

namespace pdfforge.PrinterHelper
{
    public class PrinterInfo
    {
        public string Name { get; set; }
        public string PortName { get; set; }
        public string DriverName { get; set; }
        public string DeviceId { get; set; }
        public bool Shared { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Name:       " + Name);
            sb.AppendLine("PortName:   " + PortName);
            sb.AppendLine("DriverName: " + DriverName);
            sb.AppendLine("DeviceId:   " + DeviceId);
            sb.AppendLine("Shared:     " + (Shared ? "True" : "False"));

            return sb.ToString();

        }
    }
}
