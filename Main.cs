using System;

using Postin.Aplikacja;
using Postin.Pomocnicze;

namespace Postin
{
    class Program
    {
        static void Main(string[] args)
        {
            Administrator n = new Administrator("Admin2", "1234", "A", "B", "C");
            Symulacja.zalogujUzytkownika(n);
        }
    }
}
