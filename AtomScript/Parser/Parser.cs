using System.Collections.Generic;

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
            ast = new Ast(statements);
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
            if (Match(TokenType.FOR)) return MatchForStmt();
            if (Match(TokenType.IF)) return MatchIfStmt();
            if (Match(TokenType.PRINT)) return MatchPrintStmt();
            if (Match(TokenType.RETURN)) return MatchReturnStmt();
            if (Match(TokenType.WHILE)) return MatchWhileStmt();
            if (Match(TokenType.LEFT_BRACE)) return MatchBlockStmt();

            return MatchExpressionStmt();
        }

        private ExpressionStmt MatchExpressionStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new ExpressionStmt(expr);
        }

        private ForStmt MatchForStmt() {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'for'.");
            Stmt initializer;
            if (Match(TokenType.SEMICOLON)) {
                initializer = null;
            } else if (Match(TokenType.VAR)) {
                initializer = MatchVarDeclaration();
            } else {
                initializer = MatchExpressionStmt();
            }

            Expr condition = null;
            if (Check(TokenType.SEMICOLON) == false) {
                condition = MatchExpression();
            }
            Consume(TokenType.SEMICOLON, "Expect ';' after loop condition.");

            Expr increment = null;
            if (Check(TokenType.RIGHT_PAREN) == false) {
                increment = MatchExpression();
            }
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after clauses.");

            Stmt body = MatchStatement();
            return new ForStmt(initializer, condition, increment, body);
        }

        private IfStmt MatchIfStmt() {
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

        private PrintStmt MatchPrintStmt() {
            Expr expr = MatchExpression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new PrintStmt(expr);
        }

        private ReturnStmt MatchReturnStmt() {
            Token keyword = ReadPrevToken();
            Expr value = null;
            if (Check(TokenType.SEMICOLON) == false) {
                value = MatchExpression();
            }
            Consume(TokenType.SEMICOLON, "Expect ';' after return value.");
            return new ReturnStmt(keyword, value);
        }

        private WhileStmt MatchWhileStmt() {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            Expr condition = MatchExpression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            Stmt body = MatchStatement();

            return new WhileStmt(condition, body);
        }

        private BlockStmt MatchBlockStmt() {
            List<Stmt> stmts = new List<Stmt>();
            while (Check(TokenType.RIGHT_BRACE) == false && IsAtEnd() == false) {
                stmts.Add(MatchDeclaration());
            }
            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            BlockStmt block = new BlockStmt(stmts);
            return block;
        }

        private Stmt MatchDeclaration() {
            if (Match(TokenType.CLASS)) return MatchClassDeclaration();
            if (Match(TokenType.FUNC)) return MatchFuncDeclaration("function");
            if (Match(TokenType.VAR)) return MatchVarDeclaration();
            return MatchStatement();
        }

        private ClassDeclarationStmt MatchClassDeclaration() {
            Token name = Consume(TokenType.IDENTIFIER, "Expect class name.");

            VariableExpr superclass = null;
            if (Match(TokenType.COLON)) {
                Consume(TokenType.IDENTIFIER, "Expect superclass name.");
                superclass = new VariableExpr(ReadPrevToken());
            }

            Consume(TokenType.LEFT_BRACE, "Expect '{' before class body.");

            List<FuncDeclarationStmt> methods = new List<FuncDeclarationStmt>();
            while (Match(TokenType.FUNC)) {
                methods.Add(MatchFuncDeclaration("method"));
            }
            Consume(TokenType.RIGHT_BRACE, "Expect '}' after class body.");

            return new ClassDeclarationStmt(name, superclass, methods);
        }

        private FuncDeclarationStmt MatchFuncDeclaration(string kind) {
            Token name = Consume(TokenType.IDENTIFIER, "Expect " + kind + " name.");
            Consume(TokenType.LEFT_PAREN, "Expect '(' after " + kind + " name.");
            List<Token> parameters = new List<Token>();
            if (Check(TokenType.RIGHT_PAREN) == false) {
                do {
                    if (parameters.Count >= 8) {
                        ReportError("Cannot have more than 8 parameters.");
                    }

                    parameters.Add(Consume(TokenType.IDENTIFIER, "Expect parameter name."));
                } while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters.");

            Consume(TokenType.LEFT_BRACE, "Expect '{' before " + kind + " body.");
            BlockStmt body = MatchBlockStmt();
            return new FuncDeclarationStmt(name, parameters, body);
        }

        private VarDeclarationStmt MatchVarDeclaration() {
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
                } else if (expr.GetType() == typeof(GetExpr)) {
                    GetExpr gexpr = (GetExpr)expr;
                    return new SetExpr(gexpr.obj, gexpr.name, value);
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

            return MatchCallExpr();
        }

        private Expr MatchCallExpr() {
            Expr expr = MatchPrimary();
            while (true) {
                if (Match(TokenType.LEFT_PAREN)) {
                    List<Expr> args = new List<Expr>();
                    do {
                        if (Check(TokenType.RIGHT_PAREN) == false) {
                            if (args.Count >= 8) {
                                ReportError("Cannot have more then 8 arguments.");
                            }
                            args.Add(MatchExpression());
                        }
                    } while (Match(TokenType.COMMA));
                    Token paren = Consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments.");

                    expr = new CallExpr(expr, paren, args);
                } else if (Match(TokenType.DOT)) {
                    Token name = Consume(TokenType.IDENTIFIER, "Expect property name after '.'.");
                    expr = new GetExpr(expr, name);
                } else {
                    break;
                }
            }

            return expr;
        }

        private Expr MatchPrimary() {
            if (MatchOne(new TokenType[] { TokenType.TRUE, TokenType.FALSE, TokenType.NIL, TokenType.NUMBER, TokenType.STRING })) {
                return new LiteralExpr(ReadPrevToken());
            };

            if (Match(TokenType.SUPER)) {
                Token keyword = ReadPrevToken();
                Consume(TokenType.DOT, "Expect '.' after 'super'.");
                Token method = Consume(TokenType.IDENTIFIER, "Expect superclass method name.");
                return new SuperExpr(keyword, method);
            }

            if (Match(TokenType.THIS)) return new ThisExpr(ReadPrevToken());

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
