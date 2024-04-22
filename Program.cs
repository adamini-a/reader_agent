using System.Text;
using System.Management;
internal class Program {
  private static void Main(string[] args) {
    Console.WriteLine(ListAllProcesses());
    Console.ReadLine();
  }

  public static string ListAllProcesses() {
    StringBuilder sb = new StringBuilder();
    // list out all processes and write them into a stringbuilder
    ManagementClass MgmtClass = new ManagementClass("Win32_Process");
    foreach(ManagementObject mo in MgmtClass.GetInstances()) {
      sb.Append("Name:\t" + mo["Name"] + Environment.NewLine);
      sb.Append("ID:\t" + mo["ProcessId"] + Environment.NewLine);
      sb.Append("Owner:\t" + GetProcessOwner(mo["ProcessId"].ToString()) + Environment.NewLine);
      sb.Append(Environment.NewLine);
    }
    return sb.ToString();
  }
  public static string GetProcessOwner(string ProcessId) {
    string query = "Select * from Win32_Process Where ProcessId = \"" + ProcessId + "\"";

    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
    ManagementObjectCollection processList = searcher.Get();

    foreach(ManagementObject obj in processList) {
      string[] argList = new string[] {
        string.Empty, string.Empty
      };
      int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
      if (returnVal == 0) {
        // return DOMAIN\user
        string owner = argList[1] + "\\" + argList[0];
        return owner;
      }
    }
    return "NO OWNER";
  }
}
