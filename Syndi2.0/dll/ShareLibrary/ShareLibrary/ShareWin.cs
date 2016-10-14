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
<<<<<<< HEAD
        public static List<ManagementBaseObject> GetSharedFiles()
        {
            var objClass = new System.Management.ManagementClass("Win32_Share");
            List<ManagementBaseObject> result = new List<ManagementBaseObject>();
=======
        static System.Management.ManagementClass GetSharedFiles()
        {
            var objClass = new System.Management.ManagementClass("Win32_Share");
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
            foreach (var objShare in objClass.GetInstances())
            {
                if (CheckAccess(Convert.ToString(objShare.Properties["Path"].Value)))
                {
<<<<<<< HEAD
                    result.Add(objShare);
                    //objShare.Properties["Name"].Value, objShare.Properties["Path"].Value
                }
            }
            return result;
        }

        public static Boolean CheckAccess(String filePath)
=======
                    Console.WriteLine(String.Format("{0} -> {1}",
                    objShare.Properties["Name"].Value, objShare.Properties["Path"].Value));
                }
            }
            return objClass;
        }

        private static Boolean CheckAccess(String filePath)
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
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

<<<<<<< HEAD
        public static int RemoveSharedFolder(string ShareName)
        {
            try
            {
                // Create a ManagementClass object
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementObjectCollection shares = managementClass.GetInstances();
                foreach (ManagementObject share in shares)
                {
                    if (Convert.ToString(share["Name"]).Equals(ShareName))
                    {
                        var result = share.InvokeMethod("Delete", new object[] { });

                        // Check to see if the method invocation was successful
                        if (Convert.ToInt32(result) != 0)
                        {
                            Console.WriteLine("Unable to unshare directory.");
                            return 0;
                        }
                        else
                        {
                            Console.WriteLine("Folder successfuly unshared.");
                            return 1;
                        }
                    }
                }
                return 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                return -1;
            }
        }

        public static uint CreateSharedFolder(string FolderPath, string ShareName, string Description,int AccessLvl)
        {
            try
            {
                // Create a ManagementClass object
                ManagementClass managementClass = new ManagementClass("Win32_Share");

                // Create ManagementBaseObjects for in and out parameters
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");

                ManagementBaseObject outParams;

                // Set the input parameters
=======
        static Boolean ShareWithEveryone(string FolderPath, string ShareName, string Description)
        {
            Boolean first = QshareFolder(@FolderPath,ShareName,Description);
            Boolean second = GrantEveryone(@FolderPath);
            return first && second;
        }
        private static Boolean GrantEveryone(String path)
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
        private static Boolean QshareFolder(string FolderPath, string ShareName, string Description)
        {
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
                ManagementBaseObject outParams;
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
                inParams["Description"] = Description;
                inParams["Name"] = ShareName;
                inParams["Path"] = FolderPath;
                inParams["Type"] = 0x0; // Disk Drive
<<<<<<< HEAD

                //Another Type:
                // DISK_DRIVE = 0x0
                // PRINT_QUEUE = 0x1
                // DEVICE = 0x2
                // IPC = 0x3
                // DISK_DRIVE_ADMIN = 0x80000000
                // PRINT_QUEUE_ADMIN = 0x80000001
                // DEVICE_ADMIN = 0x80000002
                // IPC_ADMIN = 0x8000003

                //inParams["MaximumAllowed"] = 2;
                inParams["Password"] = null;

                NTAccount everyoneAccount = new NTAccount(null, "EVERYONE");
                SecurityIdentifier sid = (SecurityIdentifier)everyoneAccount.Translate(typeof(SecurityIdentifier));
                byte[] sidArray = new byte[sid.BinaryLength];
                sid.GetBinaryForm(sidArray, 0);

                ManagementObject everyone = new ManagementClass("Win32_Trustee");
                everyone["Domain"] = null;
                everyone["Name"] = "EVERYONE";
                everyone["SID"] = sidArray;

                ManagementObject dacl = new ManagementClass("Win32_Ace");
                dacl["AccessMask"] = 1179817;
                dacl["AceFlags"] = 3;
                dacl["AceType"] = 0;
                dacl["Trustee"] = everyone;

                ManagementObject securityDescriptor = new ManagementClass("Win32_SecurityDescriptor");
                securityDescriptor["ControlFlags"] = 4; //SE_DACL_PRESENT 
                securityDescriptor["DACL"] = new object[] { dacl };

                inParams["Access"] = securityDescriptor;

                // Invoke the "create" method on the ManagementClass object
                outParams = managementClass.InvokeMethod("Create", inParams, null);
                if (AccessLvl == 0)
                    GrantEveryoneRead(@FolderPath);
                if (AccessLvl == 1)
                    GrantEveryoneFullControl(@FolderPath);

                // Check to see if the method invocation was successful
                var result = (uint)(outParams.Properties["ReturnValue"].Value);
                
                return result;
            }
            catch (Exception ex)
            {
                return 1;
                //Console.WriteLine("Error:" + ex.Message);
            }
        }

        public static Boolean GrantEveryoneRead(String path)
        {
            try
            {
                DirectorySecurity sec = Directory.GetAccessControl(path);
                // Using this instead of the "Everyone" string means we work on non-English systems.
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(path, sec);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static Boolean GrantEveryoneFullControl(String path)
        {
            try
            {
                DirectorySecurity sec = Directory.GetAccessControl(path);
                // Using this instead of the "Everyone" string means we work on non-English systems.
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.FullControl | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                Directory.SetAccessControl(path, sec);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

=======
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
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
    }
}
