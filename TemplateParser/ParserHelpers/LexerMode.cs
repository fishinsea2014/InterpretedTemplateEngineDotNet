using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public enum LexerMode
    {
        /// <summary>
        /// Text
        /// </summary>
        Text = 0,

        /// <summary>
        /// Tokens
        /// </summary>
        Label = 1,


        /// <summary>
        /// Formating string
        /// </summary>
        FormatString =2

    }
}
