using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphParsing
{
    public class CharsGraph
    {
        readonly Dictionary<char, List<char>> matrixDictionary;

        public CharsGraph()
        {
            matrixDictionary = new Dictionary<char, List<char>>();
        }

        public CharsGraph AddEdge(char hostChar, string neighbors)
        {
            List<char> neighborChars = neighbors.ToCharArray().ToList();

            if (matrixDictionary.ContainsKey(hostChar))
            {
                matrixDictionary[hostChar].AddRange(neighborChars);
            }
            else
            {
                matrixDictionary.Add(hostChar, neighborChars);
            }

            foreach (char newHostChar in neighborChars)
            {
                if (matrixDictionary.ContainsKey(newHostChar))
                {
                    matrixDictionary[newHostChar].AddRange(neighborChars);
                }
                else
                {
                    matrixDictionary.Add(newHostChar, new List<char> { hostChar });
                }
            }

            RemoveHostChar();
            RemoveDuplciates();

            return this;
        }
        private void RemoveDuplciates()
        {
            for (int index = 0; index < matrixDictionary.Count; index++)
            {
                var item = matrixDictionary.ElementAt(index);
                matrixDictionary[item.Key] = item.Value.Distinct().ToList();
            }
        }
        private void RemoveHostChar()
        {
            foreach (var pair in matrixDictionary)
            {
                pair.Value.RemoveAll(c => c.Equals(pair.Key));
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            foreach (var pair in matrixDictionary)
            {
                s.AppendFormat("{0}:", pair.Key);

                foreach (var neighbor in pair.Value)
                {
                    s.AppendFormat("{0},", neighbor);
                }

                s.Append(";");
            }

            RemoveUselessChars(s);

            return s.ToString();
        }
        private static void RemoveUselessChars(StringBuilder s)
        {
            s.Replace(",;", ";");
        }

        public List<string> ComputeWords()
        {
            List<Word> words = new List<Word>();

            ComputeTwoCharsWords(words);

            //for (var index = 0; index < words.Count; index++)
            //{
            //    var word = words[index];
            //    if ()
            //}

            return words.Select(w => w.ToString()).ToList();
        }

        private void ComputeTwoCharsWords(List<Word> words)
        {
            foreach (var item in matrixDictionary)
            {
                GetValue(words, item.Key, item.Value);
            }
        }

        private static void GetValue(List<Word> words, char key, List<char> neighbors)
        {
            foreach (char neighbor in neighbors)
            {
                var word = new Word($"{key}{neighbor}");

                if (!words.Contains(word))
                {
                    words.Add(word);
                }
            }
        }
    }
}
