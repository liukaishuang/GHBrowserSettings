// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

[DllImport("Dnsapi.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool DnsFlushResolverCache();
string h = "github.com";
Ping p = new Ping();
PingReply pr = p.Send(h);
if (pr == null)
{
    Console.WriteLine("Cannot connect");
}
else if (pr.Status == IPStatus.Success)
{
    string hostPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts";
    string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
    if (!Directory.Exists(tempPath))
    {
        Directory.CreateDirectory(tempPath);
    }
    tempPath += "\\hosts";
    File.Copy(hostPath, tempPath);
    File.AppendAllLines(tempPath, new string[] { Environment.NewLine, pr.Address.ToString() + " " + h });
    File.Move(hostPath, hostPath + "_backup_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), true);
    File.Move(tempPath, hostPath);

    if (DnsFlushResolverCache())
    {
        Console.WriteLine("Connection succeeded");
    }
}
Console.ReadLine();
