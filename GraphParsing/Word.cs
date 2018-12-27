using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphParsing
{
    public class Word
    {
        private readonly string w;

        public Word(string w)
        {
            this.w = w;
        }

        public override string ToString()
        {
            return w;
        }
    }
}
