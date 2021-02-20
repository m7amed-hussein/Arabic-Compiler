using System;
namespace CompileParser
{
    // all token types in the language
    public enum TokenType
    {
        Int, If, Return, While, Else, Empty, Real, Integer, IfEqual, IfNotEqual, SemiColumn,
        Column, LeftFParen, RightFParen, LeftAParen, RightAParen, 
        LeftParen, RightParen, Assign, Less, LessEqual, Greater, GreaterEqual,
        Plus, Minus, Multiply, Divide, Identifier, CharLiteral,
        Error, Eof

    }
}
