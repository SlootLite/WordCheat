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
        public char[,] charArray;
        public MainForm()
        {
            InitializeComponent();
            SQLiteDB.Connect();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            int count = 0; // количество потоков
            charArray = GetData.init(this);
            for (int i = 0; i < charArray.GetLength(0); i++)
            {
                for (int j = 0; j < charArray.GetLength(1); j++)
                {
                    WordFinderArg wfa = new WordFinderArg()
                    {
                        idx1 = i,
                        idx2 = j
                    };
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
                    bw.RunWorkerAsync(wfa);
                    count++;
                }
            }
            if(count > 0) setProgressMax(count); // укажем размер прогресс бара
        }
        public void sendResult(string text)
        {
            wordsGrid.Rows.Add("0", text);
        }
        private void setProgressMax(int max)
        {
            progressBar.Maximum = max;
        }
        public void addProgress()
        {
            progressBar.Value += 1;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WordFinderArg wfa = (WordFinderArg)e.Argument;
            WordFinder wf = new WordFinder();
            List<string> dict = wf.findWords(charArray, wfa.idx1, wfa.idx2);
            e.Result = dict;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<string> dict = (List<string>)e.Result;
            for (int k = 0; k < dict.Count; k++)
            {
                this.sendResult(dict[k]);
            }
            addProgress();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SQLiteDB.Disconnect();
        }
    }
}
