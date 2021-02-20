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
            //uncomment to get input from the console
            input = Console.ReadLine();

            //input sample
            //input = "	صحيح محمد٣  ؛    ";


            //set to an instance of an object
            Errors.LErrors = new List<string>();

            //uncomment if you want to see all tokens in the input
            //tok();



            //parse
            parser();





            //printing all errors 
            foreach(var error in Errors.LErrors)
            {
                Console.WriteLine(error);
            }
        }











        //start parsing
        public static void parser()
        {
            Parser parser = new Parser(new Lexeme(input));
            parser.Start();
        }





        //to see all tokens in the input string
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
