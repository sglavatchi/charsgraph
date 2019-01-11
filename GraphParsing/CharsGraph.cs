using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphParsing
{
    public class CharsGraph
    {
        private readonly List<string> vocabularyList;
        readonly Dictionary<char, List<char>> matrixDictionary;

        public CharsGraph(List<string> vocabularyList = null)
        {
            this.vocabularyList = vocabularyList ?? new List<string>();
            this.matrixDictionary = new Dictionary<char, List<char>>();
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
                    matrixDictionary[newHostChar].Add(hostChar);
                }
                else
                {
                    matrixDictionary.Add(newHostChar, new List<char> { hostChar });
                }
            }

            RemoveHostCharFromDependencies();
            RemoveDuplicates();

            return this;
        }
        private void RemoveDuplicates()
        {
            for (int index = 0; index < matrixDictionary.Count; index++)
            {
                var item = matrixDictionary.ElementAt(index);
                matrixDictionary[item.Key] = item.Value.Distinct().ToList();
            }
        }
        private void RemoveHostCharFromDependencies()
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
            Dictionary<char, List<string>> words = new Dictionary<char, List<string>>();

            foreach (var node in GetAllNodes())
            {
                var sortedNodes = new List<char>();
                var visitedNodes = new HashSet<char>();

                Visit(words, node, visitedNodes, sortedNodes, GetNodeDependencies());
            }

            var result = GetResult(words);

            return result;
        }
        private List<char> GetAllNodes()
        {
            List<char> nodes = new List<char>(matrixDictionary.Keys.ToList());

            foreach (List<char> matrixDictionaryValue in matrixDictionary.Values)
            {
                nodes.AddRange(matrixDictionaryValue);
            }
            return nodes.Distinct().ToList();
        }
        private Func<char, IEnumerable<char>> GetNodeDependencies()
        {
            return hostChar =>
            {
                if (matrixDictionary.ContainsKey(hostChar))
                {
                    return matrixDictionary[hostChar];
                }
                return new char[0];
            };
        }
        private void Visit(Dictionary<char, List<string>> words, char item, HashSet<char> visitedNodes, List<char> sortedNodes, Func<char, IEnumerable<char>> nodeDependencies)
        {
            if (!visitedNodes.Contains(item))
            {
                visitedNodes.Add(item);

                foreach (var dep in nodeDependencies(item))
                {
                    Visit(words, dep, visitedNodes, sortedNodes, nodeDependencies);
                }

                var word = ComputeWord(visitedNodes, sortedNodes);

                if (IsLegitimeWord(word))
                {
                    AddLegitimeWord(words, item, word);
                }

                sortedNodes.Add(item);
            }
        }
        private static string ComputeWord(HashSet<char> visitedNodes, List<char> sortedNodes)
        {
            HashSet<char> hashSet = new HashSet<char>(visitedNodes);
            hashSet.RemoveWhere(sortedNodes.Contains);
            IEnumerable<char> reverse = hashSet.Reverse();

            return $"{string.Join("", reverse)}";
        }
        private bool IsLegitimeWord(string word)
        {
           return vocabularyList.Contains(word);
        }
        private static void AddLegitimeWord(Dictionary<char, List<string>> words, char item, string word)
        {
            if (words.ContainsKey(item))
                words[item].Add(word);
            else
                words.Add(item, new List<string> { word });
        }
        private static List<string> GetResult(Dictionary<char, List<string>> words)
        {
            List<string> result = new List<string>();
            foreach (KeyValuePair<char, List<string>> word in words)
            {
                result.AddRange(word.Value);
            }
            return result;
        }

    }
}

