using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace IQMedia.Security
{
    public class Authentication
    {
        static Authentication()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CustomAssembly_Resolve);
        }

        public static string GetHashPassword(string p_Password, int p_round = 11)
        {
            if (string.IsNullOrWhiteSpace(p_Password))
            {
                throw new ArgumentException("input password must not be blank.");
            }

            if (p_round < 4 || p_round > 31)
            {
                throw new ArgumentException("Round must be within 4 and 31.");
            }

            return BCrypt.Net.BCrypt.HashPassword(p_Password, p_round);
        }

        public static bool VerifyPassword(string p_Password, string p_HashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(p_Password, p_HashPassword);
        }

        static System.Reflection.Assembly CustomAssembly_Resolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("BCrypt"))
            {
                string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

                dllName = dllName.Replace(".", "_");

                if (dllName.EndsWith("_resources")) return null;

                System.Resources.ResourceManager rm = new System.Resources.ResourceManager("IQMedia.Security.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

                byte[] bytes = (byte[])rm.GetObject(dllName);

                return System.Reflection.Assembly.Load(bytes); 
            }
            else
            {
                return null;
            }
        }
    }
}
