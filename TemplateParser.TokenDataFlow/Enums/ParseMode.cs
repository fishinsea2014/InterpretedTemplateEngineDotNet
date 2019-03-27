using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateParser
{
    public enum ParseMode
    {
        None = 0,

        /// <summary>
        /// In a tag
        /// </summary>
        EnterLabel =1,

        /// <summary>
        /// Leave a tag
        /// </summary>
        LeaveLabel =2
    }
}
