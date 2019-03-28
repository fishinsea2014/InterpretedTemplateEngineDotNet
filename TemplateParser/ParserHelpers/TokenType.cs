using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    /// <summary>
    /// Identify the type of each token stored in the list
    /// </summary>
    public enum TokenType
    {
       
        /// <summary>
        /// Left square bracket
        /// </summary>
        LeftBracket =1,

        /// <summary>
        /// Right square bracket
        /// </summary>
        RightBracket =2,

        /// <summary>
        /// General text
        /// </summary>
        Text = 3,

        /// <summary>
        /// Token
        /// </summary>
        Token = 4,
        
        /// <summary>
        /// Formate string preamble, which is '"'
        /// </summary>
        FormatStringPreamble = 5,

        /// <summary>
        /// Formate string
        /// </summary>
        FormatString =6
    }
}
