using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public class Token
    {
        /// <summary>
        /// Token construction method
        /// </summary>
        /// <param name="Type">The type of a token</param>
        /// <param name="text">The content of a token</param>
        /// <param name="line">The line position</param>
        /// <param name="column">The column position</param>
        /// <param name="parent">The cascading name of the object which a token is blong to </param>
        public Token(TokenType Type, string text, int line, int column, string parent)
        {
            this.Text = text;
            this.Kind = Type;
            this.Column = column;
            this.Line = line;
            this.Parent = parent;
        }

        public string Text { get; }
        public TokenType Kind { get; }
        public int Column { get; }
        public int Line { get; }
        public string Parent { get; }
    }
}
