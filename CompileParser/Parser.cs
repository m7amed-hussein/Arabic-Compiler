using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompileParser
{
    public class Parser
    {

         Token token;          // current token
         Lexeme lexer;
         bool flag = true;

         public Parser(Lexeme ts)
         {
             lexer = ts;
             token = lexer.nextToken();            // retrieve first Token
         }

         public void Start()
         {
             Program();
            if (!token.type.Equals(TokenType.Eof))
            {
                error(TokenType.Error);
            }
            if (flag)
                Errors.LErrors.Add("parsed successfully");
             
         }

        //program → declaration-list
        public void Program()
         {
             DeclarationList();
             

         }
        
        //declaration-list → declaration declaration-list | declaration
        void DeclarationList()
        {
            //one or more declaration
            Declaration();
            //صحیح | حقیقى | خالى → type-specifier

            while (token.type.Equals(TokenType.Integer) |
                     token.type.Equals(TokenType.Real) |
                     token.type.Equals(TokenType.Empty))
            {
                Declaration();
            }

        }

        //declaration → var-declaration | fun-declaration
        void Declaration()
        {
            //all declarations have type-specifier and Variable-Name(Identifier)
            variableType();
            variableName();
            //check if it var declaration 
            if (token.type.Equals(TokenType.SemiColumn)|| token.type.Equals(TokenType.RightAParen))
                VarDeclaration();//variable declaration
            else 
            FuncDeclaration();



        }

        //var-declaration → ؛ ID type-specifier | ؛ [Num] ID type-specifier
        void VarDeclaration()
         {

            //var or array declaration
            if (token.type.Equals(TokenType.SemiColumn))
                semicolumn();
            else
            {
                RightAParen();
                numbers();
                LeftAParen();
                semicolumn();
            }

         }
        //fun-declaration → compound-stmt(params) ID type-specifier
        void FuncDeclaration()
        {
            RightParen();
            if (token.type.Equals(TokenType.Integer) |
                     token.type.Equals(TokenType.Real) |
                     token.type.Equals(TokenType.Empty))
                ParamList();
            LeftParen();
            CompoundStmt();
        }
        //one or mare parameter
        //param-list → param-list ، param | param
        void ParamList()
        {
            param();
            while (token.type.Equals(TokenType.Column))
            {
                Column();
                param();
            }

        }
        //param → ID type-specifier | [ ] ID type-specifier
        void param()
        {
            //all declarations have type-specifier and Variable-Name
            variableType();
            variableName();
            if (token.type.Equals(TokenType.RightAParen))
            {
                RightAParen();
                LeftAParen();
            }

        }
        //compound-stmt → { stmt-list local-declarations }
        void CompoundStmt()
        {
            RightFParen();
            StmtList();
            LocalDeclarations();
            LeftFParen();

        }
        //stmt-list → statement stmt-list | ​ε
        //statement → expression-statement | compound-statement |
        //selection-statement | iteration-statement |
        //return statement
        void StmtList()
        {
            //non or more statements
            while (!token.type.Equals(TokenType.Integer) &
                     !token.type.Equals(TokenType.Real) &
                     !token.type.Equals(TokenType.Empty)&
                     !token.type.Equals(TokenType.LeftFParen)&
                     !token.type.Equals(TokenType.Eof))
            {
                Statement();
            }
                
        }
        void Statement()
        {
            if (token.type.Equals(TokenType.RightFParen))
                CompoundStmt();
            else if (token.type.Equals(TokenType.If))
                SelectionStm();
            else if (token.type.Equals(TokenType.While))
                IterationStm();
            else if (token.type.Equals(TokenType.Return))
                ReturnStm();
            else
                ExpressionStm();
        }
        //| اذا ) selection-statement → statement ( expression 
        //اذا ) statement ( expression اخر statement
        void SelectionStm()
        {
            match(TokenType.If) ;
            RightParen();
            Expression();
            LeftParen();
            Statement();
            if (token.type.Equals(TokenType.Else))
            {
                match(TokenType.Else);
                StmtList();
            }
                
        }
        //بینما ) iteration-stmt → statement ( expression 
        void IterationStm()
        {
            match(TokenType.While);
            RightParen();
            Expression();
            LeftParen();
            StmtList();
        }
        //| ارجع ؛ → return-stmt 
        //ارجع expression ؛
        void ReturnStm()
        {
            match(TokenType.Return);
            if (token.type.Equals(TokenType.SemiColumn))
                semicolumn();
            else
            {
                Expression();
                semicolumn();
            }
               

        }
        //local-declarations → var-declaration local-declarations | ​ε
        //var-declaration → ؛ ID type-specifier | ؛ [Num] ID type-specifier
        void LocalDeclarations()
        {
            //once or non
            while (token.type.Equals(TokenType.Integer) |
                     token.type.Equals(TokenType.Real) |
                     token.type.Equals(TokenType.Empty))
            {
                variableType();
                variableName();
                VarDeclaration();

            }


        }
        //؛ | expression ؛ → expression-stmt 
        void ExpressionStm()
        {
            if (token.type.Equals(TokenType.SemiColumn))
                semicolumn();
            else
            {
                Expression();
                semicolumn();
            }
                
        }
        //expression → expression = var | simple-expression
        //var → ID | ​[expression] ID
        void Expression()
        {
           
            SimpleExpression();
            if (token.type.Equals(TokenType.Assign))
            {
                Assign();
                Expression();
            }


        }
        //simple-expression → additive-expression relOp additive-expression | additive-expression
        void SimpleExpression()
        {
            AdditiveExpression();
            if(token.type.Equals(TokenType.GreaterEqual)||
                token.type.Equals(TokenType.Greater)||
                token.type.Equals(TokenType.LessEqual)||
                token.type.Equals(TokenType.Less)||
                token.type.Equals(TokenType.IfNotEqual)||
                token.type.Equals(TokenType.IfEqual))
            {
                RelOp();
                AdditiveExpression();
            }
            
        }
        //var → ID | ​[expression ] ID
        void Var()
        {
            variableName();
            if (token.type.Equals(TokenType.RightAParen))
            {
                RightAParen();
                Expression();
                LeftAParen();
            }

        }
        //relOp → =< | => | < | > | =! | ==
        void RelOp()
        {
            if (token.type.Equals(TokenType.GreaterEqual))
                match(TokenType.GreaterEqual);
            else if (token.type.Equals(TokenType.Greater))
                match(TokenType.Greater);
            else if (token.type.Equals(TokenType.LessEqual))
                match(TokenType.LessEqual);
            else if (token.type.Equals(TokenType.Less))
                match(TokenType.Less);
            else if (token.type.Equals(TokenType.IfNotEqual))
                match(TokenType.IfNotEqual);
            else if (token.type.Equals(TokenType.IfEqual))
                match(TokenType.IfEqual);

        }
        //additive-expression → term addOp additive-expression | term
        //addOp → + | -
        void AdditiveExpression()
        {
            Term();
            if (token.type.Equals(TokenType.Plus) ||
                token.type.Equals(TokenType.Minus))
            {
                AddOp();
                Term();
            }
        }
        //addOp → + | -
        void AddOp()
        {
            if (token.type.Equals(TokenType.Plus))
                match(TokenType.Plus);
            else if (token.type.Equals(TokenType.Minus))
                match(TokenType.Minus);

        }
        //term → factor mulOp term | factor
        void Term()
        {
            Factor();
            if (token.type.Equals(TokenType.Multiply)||
                token.type.Equals(TokenType.Divide))
            {
                MulOp();
                Factor();
            }
                
        }
        //mulOp → * | \
        void MulOp()
        {
            if (token.type.Equals(TokenType.Multiply))
                match(TokenType.Multiply);
            else if (token.type.Equals(TokenType.Divide))
                match(TokenType.Divide);

        }
        //factor → (expression ) | var | call | Num
        //var → ID | ​[expression ] ID
        //cal → ( args ) ID
        void Factor()
        {
            if (token.type.Equals(TokenType.RightParen))
            {
                RightParen();
                Expression();
                LeftParen();
            }else if (token.type.Equals(TokenType.Identifier)){
                variableName();
                if (token.type.Equals(TokenType.RightParen))
                    Call();
                else if (token.type.Equals(TokenType.RightAParen))
                    Var2();
            }
            else
            {
                numbers();
            }
               

        }
        //var →  ​[expression ] 
        void Var2()
        {
            if (token.type.Equals(TokenType.RightAParen))
            {
                RightAParen();
                Expression();
                LeftAParen();
            }

        }
        //cal → ( args ) ID
        void Call()
        {
            RightParen();
            Args();
            LeftParen();
        }
        //args → args-list | ε
        //args-list → expression ، args-list | expression
        void Args()
        {
            Expression();
            while (token.type.Equals(TokenType.Column))
            {
                Column();
                Expression();
            }

        }
        
        //check if it type specifier
        void variableType()
         {
             if (token.type.Equals(TokenType.Integer))
             {
                 match(TokenType.Integer);
             }
             else if (token.type.Equals(TokenType.Empty))
             {

                 match(TokenType.Empty);


             }
             else if (token.type.Equals(TokenType.Real))
             {

                 match(TokenType.Real);
             }
             else
             {
                 error(token.type);
             }
         }
        //check if it ID
         void variableName()
         {
             character();

         }
        
         void character()
         {
             match(TokenType.Identifier);
         }

         void number()
         {
             match(TokenType.Int);
         }

         void numbers()
         {
            if (token.type.Equals(TokenType.Minus))
                match(TokenType.Minus);
             number();
             while (token.type.Equals(TokenType.Int))
             {
                 number();
             }
            if (token.type.Equals(TokenType.Column))
            {
                Column();
                while (token.type.Equals(TokenType.Int))
                {
                    number();
                }
            }
            

        }
         void semicolumn()
         {
             match(TokenType.SemiColumn);
         }
        void Assign()
        {
            match(TokenType.Assign);
        }
        void RightAParen()
        {
            match(TokenType.RightAParen);
        }
        void LeftAParen()
        {
            match(TokenType.LeftAParen);
        }
        void RightParen()
        {
            match(TokenType.RightParen);
        }
        void LeftParen()
        {
            match(TokenType.LeftParen);
        }
        void RightFParen()
        {
            match(TokenType.RightFParen);
        }
        void LeftFParen()
        {
            match(TokenType.LeftFParen);
        }
        void Column()
        {
            match(TokenType.Column);
        }
        private void match(TokenType expectedToken)
         {

             //if current Token equals expected token then proceed ,otherwise fire an error
             if (token.type.Equals(expectedToken))
                 token = lexer.nextToken();
            else
            {
                error(expectedToken);
                token = lexer.nextToken();
            }
                 

         }

         private void error(TokenType t)
         {
            // true if it the first error found 
            if (flag)
            {
                flag = false;
                Errors.LErrors.Add("parsing errors:");
                
            }
            if (t.Equals(TokenType.Error))
            {
                Errors.LErrors.Add("Syntax error: expecting: diiferent token found: " + token.value + " in line :" + token.lineno);
            }
            else 
            Errors.LErrors.Add("Syntax error: expecting: " + t
                     + " found: " + token.value + " in line :" + token.lineno);
        }
        
    }
}
