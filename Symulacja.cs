using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Postin.Aplikacja;
namespace Postin.Pomocnicze
{
    public static class Symulacja
    {
        public static void zalogujUzytkownika(Uzytkownik user)
        {
            try { user.zaloguj("Admin", "1234"); }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

            }

        }
    }
}
