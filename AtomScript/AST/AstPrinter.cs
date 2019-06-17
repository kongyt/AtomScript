using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.AST.Expression;
using AtomScript.AST.Statement;
using AtomScript.Scanner;

namespace AtomScript.AST {
    class AstPrinter : AstVisitor {

        private string astStr;
        private int whiteSpaceCount;
        private bool isNewLine;
        private bool ignoreNewLine;

        public string Print(Ast ast) {
            astStr = "";
            whiteSpaceCount = 0;
            isNewLine = true;
            ast.Accept(this);
            Console.WriteLine(astStr);
            return astStr;
        }

        private void NewLine() {
            if (ignoreNewLine == false) {
                isNewLine = true;
                astStr += "\n";
            }
        }

        private string WS() {
            string rs = "";
            for (int i = 0; i < whiteSpaceCount; i++) {
                rs += " ";
            }
            return rs;
        }

        public override void Visit(AssignExpr expr) {
            astStr += expr.name.lexeme;
            astStr += " = ";
            expr.value.Accept(this);
        }

        public override void Visit(BinaryExpr expr) {
            expr.left.Accept(this);
            astStr = astStr + " " + expr.op.lexeme + " ";
            expr.right.Accept(this);
        }

        public override void Visit(CallExpr expr) {
            expr.callee.Accept(this);
            astStr = astStr + "(";
            for (int i = 0; i < expr.arguments.Count; i++) {
                expr.arguments[i].Accept(this);
                if (i != expr.arguments.Count - 1) {
                    astStr += ", ";
                }
            }
            astStr += ")";
        }

        public override void Visit(GetExpr expr) {
            expr.obj.Accept(this);
            astStr += ".";
            astStr += expr.name.lexeme;
        }

        public override void Visit(GroupingExpr expr) {
            astStr += "(";
            expr.expression.Accept(this);
            astStr += ")";
        }

        public override void Visit(LiteralExpr expr) {
            astStr += expr.literal.lexeme;
        }

        public override void Visit(LogicalExpr expr) {
            expr.left.Accept(this);
            astStr = astStr + " " + expr.op.lexeme + " ";
            expr.right.Accept(this);
        }

        public override void Visit(SetExpr expr) {
            expr.obj.Accept(this);
            astStr += ".";
            astStr += expr.name.lexeme;
            astStr += " = ";
            expr.value.Accept(this);
        }

        public override void Visit(SuperExpr expr) {
            astStr += "super.";
            astStr += expr.method.lexeme;
        }

        public override void Visit(ThisExpr expr) {
            astStr += "this.";
        }

        public override void Visit(UnaryExpr expr) {
            astStr += expr.op.lexeme;
            expr.right.Accept(this);
        }

        public override void Visit(VariableExpr expr) {
            astStr += expr.name.lexeme;
        }

        public override void Visit(BlockStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "{";
            NewLine();
            whiteSpaceCount += 4;
            for (int i = 0; i < stmt.stmts.Count; i++) {
                stmt.stmts[i].Accept(this);
            }
            whiteSpaceCount -= 4;
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "}";
            NewLine();

        }

        public override void Visit(ClassDeclarationStmt stmt) {
            astStr += "class " + stmt.name.lexeme;
            if (stmt.superclass != null) {
                astStr += " : ";
                stmt.superclass.Accept(this);
            }
            astStr += " {";
            NewLine();
            whiteSpaceCount += 4;
            for (int i = 0; i < stmt.methods.Count; i++) {
                stmt.methods[i].Accept(this);
            }
            whiteSpaceCount -= 4;
            astStr += "}";
            NewLine();
        }

        public override void Visit(ExpressionStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }      
            stmt.expr.Accept(this);
            astStr += ";";
            NewLine();
        }

        public override void Visit(ForStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            ignoreNewLine = true;
            astStr += "for(";
            if (stmt.initializer != null) {
                stmt.initializer.Accept(this);
            } else {
                astStr += ";";
            }

            if (stmt.condition != null) {
                stmt.condition.Accept(this);
            }
            astStr += ";";

            if (stmt.increment != null) {
                stmt.increment.Accept(this);
            }
            astStr += ") ";
            ignoreNewLine = false;
            if (stmt.body.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount += 4;
                NewLine();
                astStr += WS();
            }
            stmt.body.Accept(this);
            if (stmt.body.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount -= 4;
            }
        }

        public override void Visit(FuncDeclarationStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }

            astStr = astStr + "func " + stmt.name.lexeme + "(";
            for (int i = 0; i < stmt.parameters.Count; i++) {
                astStr += stmt.parameters[i].lexeme;

                if (i != stmt.parameters.Count - 1) {
                    astStr += ", ";
                }
            }

            astStr += ")";
            stmt.body.Accept(this);
        }

        public override void Visit(IfStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "if(";
            stmt.condition.Accept(this);
            astStr += ") ";

            if (stmt.thenBranch.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount += 4;
                NewLine();
            }
            stmt.thenBranch.Accept(this);
            if (stmt.thenBranch.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount -= 4;
            }

            if (stmt.elseBranch != null) {
                if (isNewLine) {
                    isNewLine = false;
                    astStr += WS();
                }
                astStr += "else ";
                if (stmt.elseBranch.GetType() != typeof(BlockStmt)) {
                    whiteSpaceCount += 4;
                    NewLine();
                }
                stmt.elseBranch.Accept(this);
                if (stmt.elseBranch.GetType() != typeof(BlockStmt)) {
                    whiteSpaceCount -= 4;
                }
            }
        }

        public override void Visit(PrintStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "print ";
            stmt.expr.Accept(this); 
            astStr += ";";
            NewLine();
        }

        public override void Visit(ReturnStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }

            astStr += "return";
            if (stmt.value != null) {
                astStr += " ";
                stmt.value.Accept(this);
            }
            astStr += ";";
            NewLine();
        }

        public override void Visit(VarDeclarationStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "var " + stmt.name.lexeme;
            if (stmt.initializer != null) {
                astStr += " = ";
                stmt.initializer.Accept(this);
                astStr += ";";
                NewLine();
            }
        }

        public override void Visit(WhileStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                astStr += WS();
            }
            astStr += "while ( ";
            stmt.condition.Accept(this);
            astStr += " ) ";
            stmt.body.Accept(this);
        }
    }
}
