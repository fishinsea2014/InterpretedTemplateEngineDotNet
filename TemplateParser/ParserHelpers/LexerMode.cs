using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    /// <summary>
    /// Identify the types of State Machine
    /// </summary>
    public enum LexerState
    {
        /// <summary>
        /// Text: current state is a general text
        /// </summary>
        Text = 0,
        /// <summary>
        /// Tokens: Current state is a token
        /// </summary>
        Label = 1,

        /// <summary>
        /// Format string: Current state is a format string
        /// </summary>
        FormatString =2

    }
}
