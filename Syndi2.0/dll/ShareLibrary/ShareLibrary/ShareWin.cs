using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;   //Added from here
using System.Security.AccessControl;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ShareLibrary
{
    public class ShareWin
    {
        public static List<ManagementBaseObject> GetSharedFiles()
        {
            var objClass = new System.Management.ManagementClass("Win32_Share");
            List<ManagementBaseObject> result = new List<ManagementBaseObject>();
            foreach (var objShare in objClass.GetInstances())
            {
                if (CheckAccess(Convert.ToString(objShare.Properties["Path"].Value)))
                {
                    result.Add(objShare);
                    //objShare.Properties["Name"].Value, objShare.Properties["Path"].Value
                }
            }
            return result;
        }

        public static Boolean CheckAccess(String filePath)
        {
            //Console.WriteLine(filePath);
            try
            {
                AuthorizationRuleCollection arcFile = File.GetAccessControl(@filePath).GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                foreach (AuthorizationRule arFile in arcFile)
                {
                    //Console.WriteLine(arFile.IdentityReference.Value);
                    if (arFile.IdentityReference.Value == "S-1-1-0")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                //Console.WriteLine("Exception Caught");
                return false;
            }

        }

        public static Boolean ShareWithEveryone(string FolderPath, string ShareName, string Description)
        {
            Boolean first = QshareFolder(@FolderPath,ShareName,Description);
            Boolean second = GrantEveryone(@FolderPath);
            return first && second;
        }
        public static Boolean GrantEveryone(String path)
        {
            try
            {
                DirectorySecurity sec = Directory.GetAccessControl(path);
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(path, sec);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
         
        }
        public static Boolean QshareFolder(string FolderPath, string ShareName, string Description)
        {
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
                ManagementBaseObject outParams;
                inParams["Description"] = Description;
                inParams["Name"] = ShareName;
                inParams["Path"] = FolderPath;
                inParams["Type"] = 0x0; // Disk Drive
                outParams = managementClass.InvokeMethod("Create", inParams, null);
                if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
                {
                    throw new Exception("Unable to share directory.");
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
