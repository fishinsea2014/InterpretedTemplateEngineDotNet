using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public enum TokenKind
    {
        None=0,

        LeftBracket =1,

        RightBracket =2,

        /// <summary>
        /// General text
        /// </summary>
        Text = 3,
        
        Label = 4,
        
        /// <summary>
        /// Formate string preamble
        /// </summary>
        FormatStringPreamble = 5,

        /// <summary>
        /// Formate string
        /// </summary>
        FormatString =6
    }
}
