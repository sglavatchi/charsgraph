using System.Collections.Generic;
using GraphParsing;
using NUnit.Framework;

namespace GraphParsingTests
{
    [TestFixture]
    public class CharsGraphTests
    {
        [Test]
        public void AddEdge_2Chars_Added()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph();

            // act
            charsGraph.AddEdge('a', "eb");

            // asserst
            string s = charsGraph.ToString();

            Assert.AreEqual("a:e,b;e:a;b:a;", s);
        }

        [Test]
        public void AddEdge_3Chars_Added()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph();

            // act
            charsGraph.AddEdge('a', "eb").AddEdge('b', "c");

            // asserst
            string s = charsGraph.ToString();

            Assert.AreEqual("a:e,b;e:a;b:a,c;c:b;", s);
        }

        [Test]
        public void AddEdge_MultiChars_Added()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph();

            // act
            charsGraph.AddEdge('a', "eb").AddEdge('b', "cde").AddEdge('c', "d").AddEdge('d', "e").AddEdge('e', "a");

            // asserst
            string s = charsGraph.ToString();

            Assert.AreEqual("a:e,b;e:a,c,d;b:a,c,d,e;c:b,d;d:b,e;", s);
        }

        [Test]
        public void ComputeWords_2Chars_Computed()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph(new List<string> { "ae", "ea", "ab", "ba" });

            // act
            charsGraph.AddEdge('a', "eb");

            // asserst
            List<string> words = charsGraph.ComputeWords();

            CollectionAssert.Contains(words, "ae");
            CollectionAssert.Contains(words, "ea");
            CollectionAssert.Contains(words, "ab");
            CollectionAssert.Contains(words, "ba");
        }

        [Test]
        public void ComputeWords_3Chars_Computed()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph(new List<string> { "ae", "ea", "ab", "ba", "bc", "cb", "abc", "cba", "bae", "eab", "eabc", "cbae" });

            // act
            charsGraph.AddEdge('a', "eb").AddEdge('b', "c");

            // asserst
            List<string> words = charsGraph.ComputeWords();

            CollectionAssert.Contains(words, "ae");
            CollectionAssert.Contains(words, "ea");

            CollectionAssert.Contains(words, "ab");
            CollectionAssert.Contains(words, "ba");

            CollectionAssert.Contains(words, "bc");
            CollectionAssert.Contains(words, "cb");

            CollectionAssert.Contains(words, "abc");
            CollectionAssert.Contains(words, "cba");

            CollectionAssert.Contains(words, "bae");
            CollectionAssert.Contains(words, "eab");

            CollectionAssert.Contains(words, "eabc");
            CollectionAssert.Contains(words, "cbae");
        }

        [Test]
        public void ComputeWords_MultiChars_Computed()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph();

            // act
            charsGraph.AddEdge('a', "eb").AddEdge('b', "cde").AddEdge('c', "d").AddEdge('d', "e").AddEdge('e', "a");

            // asserst
            List<string> words = charsGraph.ComputeWords();

            Assert.AreEqual(25, words.Count);
        }

        [Test]
        public void ComputeWords_3Chars_ComputedFiltered()
        {
            // arrange
            CharsGraph charsGraph = new CharsGraph(new List<string> { "cba", "ea" });

            // act
            charsGraph.AddEdge('a', "eb").AddEdge('b', "c");

            // asserst
            List<string> words = charsGraph.ComputeWords();

            Assert.AreEqual(2, words.Count);

            CollectionAssert.Contains(words, "cba");
            CollectionAssert.Contains(words, "ea");
        }
    }
}
