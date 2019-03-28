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


            #region Jason's first step test
            //string template = @"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with], create at [CreationTime ""yyyy年MM月dd日 HH:mm:ss""]";
            //TemplateEngine templateEngine = new TemplateEngine();
            //templateEngine.Parser(template);
            //foreach (var item in templateEngine.Tokens)
            //{
            //    Console.WriteLine($"{item.Text} -- {item.Kind}--{item.Parent}");
            //}
            //templateEngine.SetValue("FirstName", "Jason");
            //templateEngine.SetValue("Name", "CDIG");
            //templateEngine.SetValue("City", "Dalian");
            //templateEngine.SetValue("CreationTime", new DateTime(2012, 4, 3, 16, 30, 24));
            //var res = templateEngine.Process();
            //Console.WriteLine(res);
            //Console.WriteLine("=============================================");
            #endregion

            #region Jason's second step test
            string template = @"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with]";
            TemplateEngineImpl testEngine = new TemplateEngineImpl();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland"
                    }
                }
            };
            testEngine.Apply(template, dataSource);
            #endregion

            //string s = "with Content";
            //bool t=s.StartsWith("with");
            ////string ss = s.Split(' ')[1];
            //string s = "a11a";
            //var t = s.Split('.');
            //var tt = t.Take(t.Length - 1);
            //string res = String.Join(".", tt);

            Console.ReadKey();

            return 0;
        }
    }
}
