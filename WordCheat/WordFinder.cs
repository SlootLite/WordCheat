using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCheat
{
    /// <summary>
    /// Класс для передачи параметров в поток
    /// </summary>
    class WordFinderArg
    {
        public int idx1;
        public int idx2;
    }
    class WordFinder
    {
        private char[,] charArray;
        /// <summary>
        /// Начать поиск слов от указанной буквы
        /// </summary>
        /// <param name="ca">матрица букв</param>
        /// <param name="idx1">строка</param>
        /// <param name="idx2">столбец</param>
        public List<string> findWords(char[,] ca, int idx1, int idx2)
        {
            charArray = ca;
            List<string> dict = new List<string>();
            dict = goNext(charArray[idx1, idx2].ToString(), idx1.ToString() + idx2.ToString(), idx1, idx2, dict);
            return dict;
        }

        public List<string> goNext(string word, string indexes, int i, int j, List<string> dict)
        {
            int idx1 = 0, idx2 = 0;
            // вправо
            idx1 = i;
            idx2 = j + 1;
            if (charArray.GetLength(1) > idx2 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // влево
            idx1 = i;
            idx2 = j - 1;
            if (idx2 >= 0 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вверх
            idx1 = i - 1;
            idx2 = j;
            if (idx1 >= 0 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вниз
            idx1 = i + 1;
            idx2 = j;
            if (charArray.GetLength(0) > idx1 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вверх-вправо
            idx1 = i - 1;
            idx2 = j + 1;
            if (idx1 >= 0 && charArray.GetLength(1) > idx2 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вверх-влево
            idx1 = i - 1;
            idx2 = j - 1;
            if (idx1 >= 0 && idx2 >= 0 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вниз-вправо
            idx1 = i + 1;
            idx2 = j + 1;
            if (charArray.GetLength(0) > idx1 && charArray.GetLength(1) > idx2 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);

            // вниз-влево
            idx1 = i + 1;
            idx2 = j - 1;
            if (charArray.GetLength(0) > idx1 && idx2 >= 0 && !indexes.Contains(idx1.ToString() + idx2.ToString()))
                find(word, indexes, idx1, idx2, dict);
            return dict;
        }
        private void find(string word, string indexes, int idx1, int idx2, List<string> dict)
        {
            string text = (word + charArray[idx1, idx2].ToString()).ToLower();
            string textIndexes = indexes + "," + idx1.ToString() + idx2.ToString();
            DataTable result = SQLiteDB.Select("SELECT word FROM words WHERE word like '" + text + "%'");
            if (result.Rows.Count > 0)
            {
                DataTable result2 = SQLiteDB.Select("SELECT word FROM words WHERE word = '" + text + "'");
                if (result2.Rows.Count > 0)
                {
                    if (result2.Rows[0][0].ToString().Length > 1) dict.Add(result2.Rows[0][0].ToString());
                }
                dict = goNext(text, textIndexes, idx1, idx2, dict);
            }
        }
    }
}
