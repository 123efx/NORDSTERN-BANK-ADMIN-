using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nordenstern_bank
{
    public static class RoleManager
    {
        private const string ADMIN_PASS = "admin123";
        private const string MITARBEITER_PASS = "mitarbeiter123";
        private const string AZUBI_PASS = "azubi123";

        public static string UserName { get; set; }
        public static string CurrentRole { get; set; } 

        public static bool Login(string passwort)
        {
            if (passwort == ADMIN_PASS)
            {
                CurrentRole = "admin";
                return true;
            }
            if (passwort == MITARBEITER_PASS)
            {
                CurrentRole = "mitarbeiter";
                return true;
            }
            if (passwort == AZUBI_PASS)
            {
                CurrentRole = "azubi";
                return true;
            }

            return false;
        }
    }
}
