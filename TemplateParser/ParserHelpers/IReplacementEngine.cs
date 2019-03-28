using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    /// <summary>
    /// Define the basic functions of the replacement template engine
    /// </summary>
    public interface IReplacementEngine
    {
        /// <summary>
        /// Parse the module into a list of tokens
        /// </summary>
        /// <param name="templateString">The string of template</param>
        void Parser(string templateString);

        /// <summary>
        /// Convert the datasource object to a substitution keywords dictionary
        /// </summary>
        /// <param name="dataSource"></param>
        void ConvertDataSourceToDict(object dataSource);

        /// <summary>
        /// Convert the string template to a sentence with the keywords dictionary
        /// </summary>
        /// <returns></returns>
        string ProcessTemplate();
    }
}
