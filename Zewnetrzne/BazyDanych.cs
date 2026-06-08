using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Postin.Zewnetrzne
{

    public interface IDataRekord;

    static public class BazyDanych
    {
        static private Dictionary<string, List<IDataRekord>> baza_danych = new Dictionary<string, List<IDataRekord>>();

        static public List<IDataRekord> GetDataBase(string data_table_name)
        {
            if (baza_danych.ContainsKey(data_table_name)) return baza_danych[data_table_name];
            List<IDataRekord> dynamic_list = new List<IDataRekord>();
            baza_danych[data_table_name] = dynamic_list;
            return dynamic_list;        
        }

        static public void AddToBase(string dataname, IDataRekord data_rekord)
        {
            if (data_rekord == null) throw new Exception("Wrong Value");
            var dataBase = GetDataBase(dataname);
            if (dataBase!=null) dataBase.Add(data_rekord);
            else throw new Exception("No Base");
        }

        static public IDataRekord FindInBase(string dataname, Func<IDataRekord, bool> isValue)
        {
            if (isValue == null) throw new Exception("No check function");
            var dataBase = GetDataBase(dataname);
            if (dataBase!=null)
            {
                foreach(var item in dataBase)
                {
                    bool found = isValue(item);
                    if (found) return item;
                }
                return null;
            }
            else throw new Exception("No Base");
        }

        static public void PrintAllFromBase(string dataname)
        {
            var dataBase = GetDataBase(dataname);
            if (dataBase != null)
            {
                foreach (var item in dataBase)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else throw new Exception("No Base");
        }

    }
}
