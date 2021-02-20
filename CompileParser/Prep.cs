using System;
using System.Collections.Generic;
using System.Linq;

namespace CompileParser
{
    public class Lexeme
    {
        private const char eofCh = '\0';
        private const char eolnCh = '\n';
        private char ch = ' ';
        private String line = "";
        private int lineno = 0;
        private int col = 1;
        private List<String> inputL;
        


        public Lexeme(String input)
        {
            //splits the input into multiple lines
            inputL = input.Split("\n").ToList();
            

        }

        private char nextChar()
        { // Return next char
            if (ch == eofCh)
                error("Attempt to read past end of file");
            col++;
            if (col >= line.Length)
            {
                // try
                if (lineno >= inputL.Count) // at end of file
                    line = "" + eofCh;
                else
                {
                    line = inputL[lineno];
                    // System.out.println(lineno + ":\t" + line);
                    lineno++;
                    //line += eolnCh;
                } // if line
                col = 0;
            } // if col
            return line[col];
        }//nextChar


        public Token nextToken()
        {
            int temp;

            do
            {
                if (Helper.Helper.isAlpha(ch))
                {

                    String lexeme = concat(Helper.Helper.letters + Helper.Helper.digits);
                    return identifyLexeme(lexeme);
                }
                else if (Helper.Helper.isDigit(ch))
                { // int  literal
                    String number = concat(Helper.Helper.digits);

                    // int Literal
                    return new Token(TokenType.Int, number,lineno);

                }
                else switch (ch)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case eolnCh:
                            ch = nextChar();
                            break;

                        case eofCh:
                            return new Token(TokenType.Eof, "eof",lineno);

                        case '+':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.Plus, "+",temp);


                        case '-':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.Minus, "-", temp);

                        case '*':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.Multiply, "*", temp);

                        case '/':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.Divide, "/", temp);


                        case ')':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.LeftParen, "(", temp);

                        case '(':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.RightParen, ")", temp);

                        case '}':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.LeftFParen, "}", temp);

                        case '{':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.RightFParen, "{", temp);

                        case ']':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.LeftAParen, "]", temp);

                        case '[':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.RightAParen, "[", temp);
                        case '؛':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.SemiColumn, "؛",  temp ); 

                        case '،':
                            temp = lineno;
                            ch = nextChar();
                            return new Token(TokenType.Column, "،", temp); ;

                        case '=':
                            return chkOpt('=', new Token(TokenType.Assign, "=", col+1==line.Length?lineno-1:lineno),
                                    new Token(TokenType.IfEqual, "==", lineno));
                        case '!':
                            return chkOpt('=', new Token(TokenType.Error, "error", col + 1 == line.Length ? lineno - 1 : lineno),
                                    new Token(TokenType.IfNotEqual, "!=", lineno));

                        case '>':
                            return chkOpt('=', new Token(TokenType.Greater, "<", col + 1 == line.Length ? lineno - 1 : lineno),
                                    new Token(TokenType.GreaterEqual, "=<", lineno));

                        case '<':
                            return chkOpt('=', new Token(TokenType.Less, ">", col + 1 == line.Length ? lineno - 1 : lineno),
                                    new Token(TokenType.LessEqual, "=>", lineno));

                        default:
                            error("Illegal character " + ch);
                            return new Token(TokenType.Error, "error", lineno);
                    } // switch
            } while (true);

        }

        private Token chkOpt(char c, Token one, Token two)
        {

            ch = nextChar();
            if (ch != c)
                return one;
            ch = nextChar();
            return two;
        }

        private String concat(String set)
        {
            String r = "";
            do
            {
                r += ch;
                ch = nextChar();
            } while (set.IndexOf(ch) >= 0);
            return r;
        }

        Token identifyLexeme(String lexeme)
        {

            if (lexeme.Equals("صحيح")) return new Token(TokenType.Integer, lexeme, lineno);
            else if (lexeme.Equals("ارجع")) return new Token(TokenType.Return, lexeme, lineno);
            else if (lexeme.Equals("اذا")) return new Token(TokenType.If, lexeme, lineno);
            else if (lexeme.Equals("بینما")) return new Token(TokenType.While, lexeme, lineno);
            else if (lexeme.Equals("بينما")) return new Token(TokenType.While, lexeme, lineno);
            else if (lexeme.Equals("اخر")) return new Token(TokenType.Else, lexeme, lineno);
            else if (lexeme.Equals("حقیقى")) return new Token(TokenType.Real, lexeme, lineno);
            else if (lexeme.Equals("حقيقي")) return new Token(TokenType.Real, lexeme, lineno);
            else if (lexeme.Equals("خالى")) return new Token(TokenType.Empty, lexeme, lineno);
            else if (lexeme.Equals("خالي")) return new Token(TokenType.Empty, lexeme, lineno);
            else if (Helper.Helper.isID(lexeme)) return new Token(TokenType.Identifier, lexeme, lineno);
            else return new Token(TokenType.Error, lexeme, lineno);
        }

        public void error(String msg)
        {
            // adding error logs to LErors

            //Adding the column number
            Errors.LErrors.Add("Error: column " + col + " " +msg);
            //Adding the line number
            Errors.LErrors.Add("Error: Line " + lineno);
            ch = nextChar();
           
            
        }
        
    }
}
