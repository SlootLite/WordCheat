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
            this.clearResult(); // очистим предыдущий результат
            charArray = GetData.init(this); // заберем данные из полей
            for (int i = 0; i < charArray.GetLength(0); i++) // цикл по строкам
            {
                for (int j = 0; j < charArray.GetLength(1); j++) // цикл по столбцам
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
            if(count > 0) this.setProgressMax(count); // укажем размер прогресс бара
        }
        /// <summary>
        /// Очистит результат
        /// </summary>
        private void clearResult()
        {
            wordsGrid.Rows.Clear();
        }
        /// <summary>
        /// Добавит новую строку в результат
        /// </summary>
        /// <param name="key">Путь который прошел для данного слова</param>
        /// <param name="text">Слово</param>
        private void sendResult(string key, string text)
        {
            wordsGrid.Rows.Add(key, text);
        }
        /// <summary>
        /// Установит максимальное значение для прогресс бара
        /// </summary>
        /// <param name="max"></param>
        private void setProgressMax(int max)
        {
            progressBar.Maximum = max;
        }
        private void addProgress()
        {
            progressBar.Value += 1;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WordFinderArg wfa = (WordFinderArg)e.Argument;
            WordFinder wf = new WordFinder();
            Dictionary<string, string> dict = wf.findWords(charArray, wfa.idx1, wfa.idx2);
            e.Result = dict;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dictionary<string, string> dict = (Dictionary<string, string>)e.Result;
            foreach(var item in dict)
            {
                this.sendResult(item.Key, item.Value);
            }
            this.addProgress();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SQLiteDB.Disconnect();
        }
    }
}
