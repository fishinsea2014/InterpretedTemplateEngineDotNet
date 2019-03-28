using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateParser.Test {
    class Program {
        public static int Main() {


            //F5 Run this if you can't get the integrated Test Explorer to work
            //TemplateEngineImplTest test = new TemplateEngineImplTest();
            //test.Test_basic_local_property_substitute();
            //test.Test_scoped_property_substitute();
            //test.Test_spanned_local_property_substitute();
            //test.Test_invalid_property_substitute();
            //test.Test_no_property_substitute();
            //test.Test_formatted_date_property_substitute();
            //test.Test_unformattable_property_substitute();
            //Console.WriteLine("All tests passed successfully!");
            string template = @"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with], create at [CreationTime ""yyyy年MM月dd日 HH:mm:ss""]";
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.Parser(template);
            foreach (var item in templateEngine.Tokens)
            {
                Console.WriteLine($"{item.Text} -- {item.Kind}");
            }
            templateEngine.SetValue("FirstName", "Jason");
            templateEngine.SetValue("Name", "CDIG");
            templateEngine.SetValue("City", "Dalian");
            templateEngine.SetValue("CreationTime", new DateTime(2012, 4, 3, 16, 30, 24));
            var res = templateEngine.Process();
            Console.WriteLine(res);
            Console.WriteLine("=============================================");

            Console.ReadKey();

            return 0;
        }
    }
}
