using System;
using System.Collections.Generic;

using AtomScript.Scanner;
using AtomScript.AST;
using AtomScript.AST.Expression;
using AtomScript.AST.Statement;

namespace AtomScript.Parser {

    class Parser {
        private List<Token> tokens;
        private int current;
        private bool success;
        private List<SyntaxError> errors;
        private Ast ast;

        public ParseResult Parse(List<Token> tokens) {
            Reset();
            this.tokens = tokens;
            List<Stmt> statements = new List<Stmt>();
            while (IsAtEnd() == false) {
                statements.Add(MatchDeclaration());
            }
            ast = new Ast(new BlockStmt(statements));
            return new ParseResult(success, ast, errors);
        }

        private void Reset() {
            tokens = null;
            current = 0;
            success = true;
            errors = new List<SyntaxError>();
        }

        private bool MatchOne(TokenType[] types) {
            for (int i = 0; i < types.Length; i++) {
                if (Match(types[i])) {
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type) {
            if (IsAtEnd()) return false;
            return ReadToken().type == type;
        }

        private bool Match(TokenType type) {
            if (IsAtEnd()) return false;
            if (ReadToken().type == type) {
                Advance();
                return true;
            } else {
                return false;
            }
        }

        private Token Advance() {
            if (!IsAtEnd()) current++;
            return ReadPrevToken();
        }

        private bool IsAtEnd() {
            return ReadToken().type == TokenType.EOF;
        }

        private Token ReadToken() {
            return tokens[current];
        }

        private Token ReadPrevToken() {
            return tokens[current - 1];
        }

        private Stmt MatchStatement() {
            if (Match(TokenType.IF)) return MatchIfStmt();
            if (Match(TokenType.PRINT)) return MatchPrintStmt();
            if (Match(TokenType.WHILE)) return MatchWhileStmt();
            if (Match(TokenType.LEFT_BRACE)) return MatchBlockStmt();

            return MatchExpressionStmt();
        }

        private Stmt MatchExpressionStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new ExpressionStmt(expr);
        }

        private Stmt MatchIfStmt() {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            Expr condition = MatchExpression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition.");

            Stmt thenBranch = MatchStatement();
            Stmt elseBranch = null;
            if (Match(TokenType.ELSE)) {
                elseBranch = MatchStatement();
            }

            return new IfStmt(condition, thenBranch, elseBranch);
        }

        private Stmt MatchPrintStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new PrintStmt(expr);
        }

        private Stmt MatchWhileStmt() {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            Expr condition = MatchExpression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            Stmt body = MatchStatement();

            return new WhileStmt(condition, body);
        }

        private Stmt MatchBlockStmt() {
            List<Stmt> stmts = new List<Stmt>();
            while (Check(TokenType.RIGHT_BRACE) == false && IsAtEnd() == false) {
                stmts.Add(MatchDeclaration());
            }
            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            BlockStmt block = new BlockStmt(stmts);
            return block;
        }

        private Stmt MatchDeclaration() {
            if (Match(TokenType.VAR)) return MatchVarDeclaration();
            return MatchStatement();
        }

        private Stmt MatchVarDeclaration() {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.EQUAL)) {
                initializer = MatchExpression();
            }


            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new VarDeclarationStmt(name, initializer);
        }

        private Expr MatchExpression() {
            return MatchAssignment();
        }

        private Expr MatchAssignment() {
            Expr expr = MatchEquality();

            if (Match(TokenType.EQUAL)) {
                Token equal = ReadPrevToken();
                Expr value = MatchAssignment();

                if (expr.GetType() == typeof(VariableExpr)) {
                    Token name = ((VariableExpr)expr).name;
                    return new AssignExpr(name, value);
                }

                ReportError("Invalid assignment target.");
            }

            return expr;
        }

        private Expr MatchLogicOr() {
            Expr expr = MatchLogicAnd();

            while (Match(TokenType.OR)) {
                Token op = ReadPrevToken();
                Expr right = MatchLogicAnd();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchLogicAnd() {
            Expr expr = MatchEquality();

            while (Match(TokenType.AND)) {
                Token op = ReadPrevToken();
                Expr right = MatchEquality();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchEquality() {
            Expr expr = MatchComparison();

            while (MatchOne(new TokenType[] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL })) {
                Token op = ReadPrevToken();
                Expr right = MatchComparison();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchComparison() {
            Expr expr = MatchAddition();

            while (MatchOne(new TokenType[] { TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL })) {
                Token op = ReadPrevToken();
                Expr right = MatchAddition();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchAddition() {
            Expr expr = MatchMultiplication();

            while (MatchOne(new TokenType[] { TokenType.MINUS, TokenType.PLUS })) {
                Token op = ReadPrevToken();
                Expr right = MatchMultiplication();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchMultiplication() {
            Expr expr = MatchUnary();

            while (MatchOne(new TokenType[] { TokenType.SLASH, TokenType.STAR })) {
                Token op = ReadPrevToken();
                Expr right = MatchUnary();
                expr = new BinaryExpr(expr, op, right);
            }

            return expr;
        }

        private Expr MatchUnary() {
            if (MatchOne(new TokenType[] { TokenType.BANG, TokenType.MINUS })) {
                Token op = ReadPrevToken();
                Expr right = MatchUnary();
                return new UnaryExpr(op, right);
            }

            return MatchPrimary();
        }

        private Expr MatchPrimary() {
            if (MatchOne(new TokenType[] { TokenType.TRUE, TokenType.FALSE, TokenType.NIL, TokenType.NUMBER, TokenType.STRING })) {
                return new LiteralExpr(ReadPrevToken());
            };

            if (Match(TokenType.IDENTIFIER)) return new VariableExpr(ReadPrevToken());

            if (Match(TokenType.LEFT_PAREN)) {
                Expr expr = MatchExpression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new GroupingExpr(expr);
            }

            ReportError("Expect expression.");
            return null;
        }

        private Token Consume(TokenType type, string message) {
            if (Check(type)) {
                return Advance();
            } else {
                ReportError(message);
                return null;
            }            
        }

        private void ReportError(string message) {
            Token prevToken = ReadPrevToken();
            string whereStr = prevToken.type == TokenType.EOF ? "at end, " : ("at '" + prevToken.lexeme + "', ");
            SyntaxError error = new SyntaxError(prevToken.line, prevToken.column, "SyntaxError: " + whereStr + message);
            errors.Add(error);
            success = false;

            Synchronize();
        }

        private void Synchronize() {
            Advance();

            while (!IsAtEnd()) {
                if (ReadPrevToken().type == TokenType.SEMICOLON) return;

                switch (ReadToken().type) {
                    case TokenType.CLASS:
                    case TokenType.FUNC:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }
    }
}
