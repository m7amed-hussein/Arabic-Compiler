using System;
namespace CompileParser
{
    public class Token
    {
        public TokenType type;
        public String value;
        public int lineno;

        public Token(TokenType type, String value,int lineno)
        {
            this.type = type;
            this.value = value;
            this.lineno = lineno;
        }
    }
}
