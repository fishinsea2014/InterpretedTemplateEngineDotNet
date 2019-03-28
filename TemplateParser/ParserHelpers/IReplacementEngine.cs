using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public interface IReplacementEngine
    {

        void Parser(string templateString);

        void SetValue(string key, object value);

        void ConvertDataSourceToDict(object dataSource);

        string Process();
    }
}
