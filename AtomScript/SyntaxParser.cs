﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {

    class SyntaxError {
        public int line;
        public int column;
        public string errorStr;

        public SyntaxError(int line, int column, string errorStr) {
            this.line = line;
            this.column = column;
            this.errorStr = errorStr;
        }
    }

    class SyntaxResult {
        public bool success;
        public List<SyntaxError> errors;
        public List<Stmt> statements;
    }

    class SyntaxParser {
        private List<Token> tokens;
        private int current;
        private bool success;
        private List<SyntaxError> errors;
        private Stmt statements;

        public SyntaxResult Parse(List<Token> tokens) {
            Reset();
            this.tokens = tokens;
            List<Stmt> statements = new List<Stmt>();
            while (IsAtEnd() == false) {
                statements.Add(MatchDeclaration());
            }
            SyntaxResult result = new SyntaxResult();
            result.success = success;
            result.errors = errors;
            result.statements = statements;
            return result;
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
            if (Match(TokenType.LEFT_BRACE)) return MatchBlockStmt();

            return MatchExpressionStmt();
        }

        private Stmt MatchExpressionStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new Expression(expr);
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

            return new If(condition, thenBranch, elseBranch);
        }

        private Stmt MatchPrintStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Print(expr);
        }

        private Stmt MatchBlockStmt() {
            List<Stmt> stmts = new List<Stmt>();
            while (Check(TokenType.RIGHT_BRACE) == false && IsAtEnd() == false) {
                stmts.Add(MatchDeclaration());
            }
            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            Block block = new Block(stmts);
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
            return new Var(name, initializer);
        }

        private Expr MatchExpression() {
            return MatchAssignment();
        }

        private Expr MatchAssignment() {
            Expr expr = MatchEquality();

            if (Match(TokenType.EQUAL)) {
                Token equal = ReadPrevToken();
                Expr value = MatchAssignment();

                if (expr.GetType() == typeof(Variable)) {
                    Token name = ((Variable)expr).name;
                    return new Assign(name, value);
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
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchLogicAnd() {
            Expr expr = MatchEquality();

            while (Match(TokenType.AND)) {
                Token op = ReadPrevToken();
                Expr right = MatchEquality();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchEquality() {
            Expr expr = MatchComparison();

            while (MatchOne(new TokenType[] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL })) {
                Token op = ReadPrevToken();
                Expr right = MatchComparison();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchComparison() {
            Expr expr = MatchAddition();

            while (MatchOne(new TokenType[] { TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL })) {
                Token op = ReadPrevToken();
                Expr right = MatchAddition();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchAddition() {
            Expr expr = MatchMultiplication();

            while (MatchOne(new TokenType[] { TokenType.MINUS, TokenType.PLUS })) {
                Token op = ReadPrevToken();
                Expr right = MatchMultiplication();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchMultiplication() {
            Expr expr = MatchUnary();

            while (MatchOne(new TokenType[] { TokenType.SLASH, TokenType.STAR })) {
                Token op = ReadPrevToken();
                Expr right = MatchUnary();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        private Expr MatchUnary() {
            if (MatchOne(new TokenType[] { TokenType.BANG, TokenType.MINUS })) {
                Token op = ReadPrevToken();
                Expr right = MatchUnary();
                return new Unary(op, right);
            }

            return MatchPrimary();
        }

        private Expr MatchPrimary() {
            if (Match(TokenType.TRUE)) return new Literal(true);
            if (Match(TokenType.FALSE)) return new Literal(false);
            if (Match(TokenType.NIL)) return new Literal(null);

            if (MatchOne(new TokenType[] { TokenType.NUMBER, TokenType.STRING })) {
                return new Literal(ReadPrevToken().literal);
            };

            if (Match(TokenType.IDENTIFIER)) return new Variable(ReadPrevToken());

            if (Match(TokenType.LEFT_PAREN)) {
                Expr expr = MatchExpression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Grouping(expr);
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