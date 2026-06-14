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

        public static string TAG = " ////////////// ";
        public static void PrintInLine(params string[] info)
        {
            string _s = TAG;
            foreach (string s in info)
            {
                _s += s;
            }
            Console.WriteLine(_s + TAG);
        }

        public static void MakeBreakLine()
        {
            Console.WriteLine("\n\n");
        }


        /// <summary>
        /// Akcje NIE MOGĄ zawierać żadnych argumentów
        /// Akcje muszę zwracać BOOL!
        /// </summary>
        /// <typeparam name="ExceptionType">Wybierz interesujący cię błąd do wyłapania</typeparam>
        /// <param name="expected_action">Funkcja która wykonuje inną akcje (nie może przyjmować argumentów)</param>
        /// <param name="success_message">Wiadomość sukcesu</param>
        /// <param name="custom_error_message">Wiadomość porażki (opcjonalne)</param>
        public static void hookAction<ExceptionType>(Func<bool> expected_action, string success_message, string custom_error_message = "") where ExceptionType : Exception
        {
            if (expected_action == null || success_message == null)
            {
                Console.WriteLine("Błędne wykorzystanie funkcji zakotwiczenia zadania!");
                return;
            }
            // Akcje będą tutaj testowane
            bool action_status = false;
            try { action_status = expected_action(); }
            catch (ExceptionType e)
            {
                if (custom_error_message != "")
                {
                    Console.WriteLine(custom_error_message);
                }
                else
                {
                    Console.WriteLine(e.Message);
                }

            }
            finally
            {
                if (action_status) Console.WriteLine(success_message);
            }


        }
    }
}
