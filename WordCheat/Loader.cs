using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordCheat
{
    class Loader
    {
        public static int init(string text)
        {
            int count = 0;
            string[] arr = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < arr.Length; i++)
            {
                string temp = arr[i].Replace(" ", "").ToLower();
                temp = Regex.Replace(temp, "[^А-Яа-я]", "");
                if (temp.Length > 1)
                {
                    DataTable result = SQLiteDB.Select("SELECT word FROM words WHERE word='"+temp+"'");
                    if (result.Rows.Count <= 0)
                    {
                        SQLiteDB.ExecQuery("INSERT INTO words (word) VALUES('" + temp + "')");
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
