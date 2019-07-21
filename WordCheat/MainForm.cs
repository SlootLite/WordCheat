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
        public char[,] charArray; // массив букв
        private List<Color> colors = new List<Color>(); // цвета для подсветки букв
        private Graphics graph; // поле для рисования линий между текстбоксами
        public MainForm()
        {
            InitializeComponent();
            SQLiteDB.Connect();
            this.initColors();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            addWordsToolStripMenuItem.Enabled = false;
            buttonGo.Enabled = false;
            int count = 0; // количество потоков
            this.clearResult(); // очистим предыдущий результат
            this.clearBackgroundTB(); // очистим фон у текстбоксов
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
        /// <summary>
        /// Добавит к прогресс бару 1 очко
        /// </summary>
        private void addProgress()
        {
            if (progressBar.Maximum > progressBar.Value)
                progressBar.Value += 1;
            else
                this.completeFinding();
        }

        /// <summary>
        /// Сработает когда произойдет завершение поиска
        /// </summary>
        private void completeFinding()
        {
            addWordsToolStripMenuItem.Enabled = true;
            buttonGo.Enabled = true;
        }

        /// <summary>
        /// Заполнит массив цветов для подсветки букв
        /// </summary>
        private void initColors()
        {
            int r = 100, g = 255, idxColor = 30;
            bool changeRed = true;
            for (int i = 0; i < 20; i++)
            {
                colors.Add(Color.FromArgb(r, g, 0));
                if (changeRed)
                {
                    r += idxColor;
                    if (r > 255)
                    {
                        g = g - (r - 255);
                        r = 255;
                        changeRed = false;
                    }
                }
                else
                {
                    g -= idxColor;
                    if (g < 0) break;
                }
            }
        }

        /// <summary>
        /// Установит белый фон для текстбоксов
        /// </summary>
        private void clearBackgroundTB()
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(TextBox) && c.Name.Contains("charTB"))
                {
                    c.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular);
                    
                    c.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Подтсветит текст в текстбоксах
        /// </summary>
        /// <param name="path">Коды полей через запятую</param>
        private void setPathToWord(string path)
        {
            this.clearBackgroundTB();
            string[] arrIndex = path.Replace(" ","").Split(',');
            string prevIndex = "";
            graph = CreateGraphics();
            graph.Clear(this.BackColor);
            
            for (int i = 0; i < arrIndex.Length; i++)
            {
                this.setColorToChar(i, arrIndex[i]);

                if(prevIndex.Length != 0) this.drawLineToChars(prevIndex, arrIndex[i]);
                prevIndex = arrIndex[i];
            }
        }

        /// <summary>
        /// Нарисует линию от одного текстбокса до другого
        /// </summary>
        /// <param name="charCode1">код текстбокса 1</param>
        /// <param name="charCode2">код текстбокса 2</param>
        private void drawLineToChars(string charCode1, string charCode2)
        {
            TextBox tb1 = null, tb2 = null;
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    if(c.Name == "charTB" + charCode1) tb1 = (TextBox)c;
                    else if (c.Name == "charTB" + charCode2) tb2 = (TextBox)c;
                }
            }
            if(tb1 != null && tb2 != null)
            {
                int x1 = tb1.Location.X + (tb1.Size.Width / 2),
                    y1 = tb1.Location.Y + (tb1.Size.Height / 2),
                    x2 = tb2.Location.X + (tb2.Size.Width / 2),
                    y2 = tb2.Location.Y + (tb2.Size.Height / 2);
                graph.DrawLine(new Pen(Color.Black, 3), x1, y1, x2, y2);
            }
        }

        /// <summary>
        /// Подсветит конкретную букву
        /// </summary>
        /// <param name="idx">Позиция буквы по порядку</param>
        /// <param name="charCode">Код поля</param>
        private void setColorToChar(int idx, string charCode)
        {
            int idxColor = idx > colors.Count ? colors.Count - 1 : idx;
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(TextBox) && c.Name == "charTB"+ charCode)
                {
                    if(idx == 0) c.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold | FontStyle.Underline);
                    c.BackColor = colors[idxColor];
                    break;
                }
            }
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
            Dispose();
        }

        private void wordsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(wordsGrid.Rows.Count > e.RowIndex && e.RowIndex >= 0)
                this.setPathToWord(wordsGrid.Rows[e.RowIndex].Cells["key"].Value.ToString());
        }

        private void addWordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddWords addWordsForm = new AddWords();
            addWordsForm.ShowDialog();
        }
    }
}
