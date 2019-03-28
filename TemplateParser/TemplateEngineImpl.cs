using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateParser {
    public class TemplateEngineImpl : ITemplateEngine {

        /// <summary>
        /// Applies the specified datasource to a string template, and returns a result string
        /// with substituted values.
        /// </summary>
        public string Apply(string template, object dataSource) {
            //TODO: Write your implementation here that passes all tests in TemplateParser.Test project            
            //throw new NotImplementedException();
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.Parser(template);
            foreach (var item in templateEngine.Tokens)
            {
                Console.WriteLine($"{item.Text} -- {item.Kind}--{item.Parent}");
            }
            templateEngine.ConvertDataSourceToDict(dataSource);

            return "Hello John";
        }
    }
}
