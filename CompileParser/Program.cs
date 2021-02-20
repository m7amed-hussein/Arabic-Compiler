using System;
using System.Collections.Generic;
using CompileParser.Helper;

namespace CompileParser
{
    class Program
    {
        static string input;
        static void Main(string[] args)
        {
            //string input = Console.ReadLine();
            //input = " صحيح محمد٣  [3] ؛  \n  صحيح محمد٣   ) صحيح م٣   (، صحيح ؛ \n صحيح محمد٣  ؛";
            //input = "صحيح العمر بب ؛==";
            input = "حقيقي الاهلي() { { } { } { } { } { } { } }\nحقيقي الاهلي() {\nاذا(4)\n}\nحقيقي الاهلي() {\nاذا(شعبول)؛\n}";
            //input = "خالي سيد (){ اذا (عيد > ابراهيم)  ايحاجه = -٥٥،٥٥؛}حقيقي محمد ()f{حقي =؛صحيح محمذ = ١٠؛}";
            //input = "خالي سيد (){ محمد = -٨٨،٩؛ اذا (عيد > ابراهيم)  ايحاجه = -٥٥،٥٥؛ اخر اذا (سيد >= -٤)ايحاجه = احاجه؛ اخر ارجع ؛}حقيقي محمد (){حقي =٩؛صحيح محمذ ؛}";
            // input = "خالي سيد (){بينما(محمد== ١٠){اذا (سيد < ابراهيم) ارجع؛ اخر سعد = ١٢؛اذا (عيد > ابراهيم) ارجع؛ اذا (سيد >= ٤)ايحاجه = أي حاجه؛ اخر ارجع ؛}}";
            Errors.LErrors = new List<String>();
            tok();
            parser();

            foreach(var error in Errors.LErrors)
            {
                Console.WriteLine(error);
            }
        }

        public static void parser()
        {
            Parser parser = new Parser(new Lexeme(input));
            parser.Start();
        }

        public static void tok()
        {
            Lexeme lexer = new Lexeme(input);
            Token token;

            token = lexer.nextToken();
            while (token.type != TokenType.Eof)
            {
                
                Console.WriteLine(token.value + "  " + token.type+"  "+token.lineno);
                token = lexer.nextToken();


            }

            Console.WriteLine("end of code ");
        }
    }
}
