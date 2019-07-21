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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            char[,] charArray = GetData.init(this);


        }
        public void sendResult(string text)
        {
            if (resultTB.Text.Length > 0) resultTB.Text += "\r\n";
            resultTB.Text += text;
        }
    }
}
