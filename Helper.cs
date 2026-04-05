using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nordenstern_bank
{
    public static class Helper
    {
        public static string CreateIBAN()
        {
            Random rnd = new Random();
            return "DE" + rnd.Next(100000000, 999999999).ToString() + rnd.Next(1000, 9999).ToString();
        }

        public static string CreateKartenNummer()
        {
            Random rnd = new Random();
            return "53" + rnd.Next(1000, 9999).ToString() +
                         rnd.Next(1000, 9999).ToString() +
                         rnd.Next(1000, 9999).ToString() +
                         rnd.Next(1000, 9999).ToString();
        }

        public static string CreatePassword()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 9999).ToString();
        }
    }
}
