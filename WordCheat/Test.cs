using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCheat
{
    class Test
    {
        public static void init()
        {
            using (StreamReader fs = new StreamReader(@"dict.txt"))
            {
                SQLiteDB.Connect();
                while (true)
                {
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    temp = temp.Replace(" ", "").ToLowerInvariant();
                    if(temp.Length > 1)
                        SQLiteDB.ExecQuery("INSERT INTO words (word) VALUES('"+temp+"')");
                }
                SQLiteDB.Disconnect();
            }
        }
    }
}
