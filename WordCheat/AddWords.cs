using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordCheat
{
    public partial class AddWords : Form
    {
        public AddWords()
        {
            InitializeComponent();
        }

        private void add_Click(object sender, EventArgs e)
        {
            string newWords = newWordsTB.Text;
            int count = 0;
            if (newWords.Length > 0)
                count = Loader.init(newWords);
            else
                MessageBox.Show("Вы не указали слова");
            MessageBox.Show("Количество добавленных слов: "+ count);
        }
    }
}
