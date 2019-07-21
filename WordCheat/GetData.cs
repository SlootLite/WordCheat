using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordCheat
{
    class GetData
    {
        public static char[,] init(MainForm form)
        {
            char[,] charArray = new char[5, 5];
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    foreach (Control c in form.Controls)
                    {
                        if (c.GetType() == typeof(TextBox))
                        {
                            if (c.Name == "charTB"+i+j)
                            {
                                charArray[i, j] = Convert.ToChar(c.Text);
                                form.sendResult(charArray[i, j].ToString());
                            }
                        }
                    }
                }
            }
            return charArray;
        }
    }
}
