using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public class Token
    {

        public Token(TokenKind kind, string text, int line, int column, string parent)
        {
            this.Text = text;
            this.Kind = kind;
            this.Column = column;
            this.Line = line;
            this.Parent = parent;
        }

        public string Text { get; }
        public TokenKind Kind { get; }
        public int Column { get; }
        public int Line { get; }
        public string Parent { get; }
    }
}
