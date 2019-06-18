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
            AddStr("\n");
            AddStr("======================Code Begin====================\n");

            whiteSpaceCount = 0;
            isNewLine = true;
            ast.Accept(this);

            AddStr("=======================Code End=====================\n");
            AddStr("\n");
            
            return astStr;
        }

        public void AddStr(string str) {
            astStr += str;
            Console.Write(str);
        }

        private void NewLine() {
            if (ignoreNewLine == false) {
                isNewLine = true;
                AddStr("\n");
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
            string tempStr = expr.name.lexeme;
            AddStr(expr.name.lexeme);
            AddStr(" = ");

            expr.value.Accept(this);
        }

        public override void Visit(BinaryExpr expr) {
            expr.left.Accept(this);
            AddStr(" " + expr.op.lexeme + " ");

            expr.right.Accept(this);
        }

        public override void Visit(CallExpr expr) {
            expr.callee.Accept(this);
            AddStr("(");

            for (int i = 0; i < expr.arguments.Count; i++) {
                expr.arguments[i].Accept(this);
                if (i != expr.arguments.Count - 1) {
                    AddStr(", ");
                }
            }
            AddStr(")");
        }

        public override void Visit(GetExpr expr) {
            expr.obj.Accept(this);
            AddStr("." + expr.name.lexeme);
        }

        public override void Visit(GroupingExpr expr) {
            AddStr("(");
            expr.expression.Accept(this);
            AddStr(")");
        }

        public override void Visit(LiteralExpr expr) {
            AddStr(expr.literal.lexeme);
        }

        public override void Visit(LogicalExpr expr) {
            expr.left.Accept(this);
            AddStr(" " + expr.op.lexeme + " ");
            expr.right.Accept(this);
        }

        public override void Visit(SetExpr expr) {
            expr.obj.Accept(this);
            AddStr("." + expr.name.lexeme + " = ");
            expr.value.Accept(this);
        }

        public override void Visit(SuperExpr expr) {
            AddStr("super." + expr.method.lexeme );
        }

        public override void Visit(ThisExpr expr) {
            AddStr("this.");
        }

        public override void Visit(UnaryExpr expr) {
            AddStr(expr.op.lexeme);
            expr.right.Accept(this);
        }

        public override void Visit(VariableExpr expr) {
            AddStr(expr.name.lexeme);
        }

        public override void Visit(BlockStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("{");
            NewLine();
            whiteSpaceCount += 4;
            for (int i = 0; i < stmt.stmts.Count; i++) {
                stmt.stmts[i].Accept(this);
            }
            whiteSpaceCount -= 4;
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("}");
            NewLine();

        }

        public override void Visit(ClassDeclarationStmt stmt) {
            AddStr("class " + stmt.name.lexeme);
            if (stmt.superclass != null) {
                AddStr(" : ");
                stmt.superclass.Accept(this);
            }
            AddStr(" {");
            NewLine();
            whiteSpaceCount += 4;
            for (int i = 0; i < stmt.methods.Count; i++) {
                stmt.methods[i].Accept(this);
            }
            whiteSpaceCount -= 4;
            AddStr("}");
            NewLine();
        }

        public override void Visit(ExpressionStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }      
            stmt.expr.Accept(this);
            AddStr(";");
            NewLine();
        }

        public override void Visit(ForStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            ignoreNewLine = true;
            AddStr("for(");

            if (stmt.initializer != null) {
                stmt.initializer.Accept(this);
            } else {
                AddStr(";");
            }

            if (stmt.condition != null) {
                stmt.condition.Accept(this);
            }
            AddStr(";");

            if (stmt.increment != null) {
                stmt.increment.Accept(this);
            }
            AddStr(") ");
            ignoreNewLine = false;
            if (stmt.body.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount += 4;
                NewLine();
                AddStr(WS());
            }
            stmt.body.Accept(this);
            if (stmt.body.GetType() != typeof(BlockStmt)) {
                whiteSpaceCount -= 4;
            }
        }

        public override void Visit(FuncDeclarationStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("func " + stmt.name.lexeme + "(");
            for (int i = 0; i < stmt.parameters.Count; i++) {
                AddStr(stmt.parameters[i].lexeme);

                if (i != stmt.parameters.Count - 1) {
                    AddStr(", ");
                }
            }

            AddStr(")");
            stmt.body.Accept(this);
        }

        public override void Visit(IfStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("if(");
            stmt.condition.Accept(this);
            AddStr(") ");

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
                    AddStr(WS());
                }
                AddStr("else ");
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
                AddStr(WS());
            }
            AddStr("print ");
            stmt.expr.Accept(this);
            AddStr(";");
            NewLine();
        }

        public override void Visit(ReturnStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }

            AddStr("return");
            if (stmt.value != null) {
                AddStr(" ");
                stmt.value.Accept(this);
            }
            AddStr(";");
            NewLine();
        }

        public override void Visit(VarDeclarationStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("var " + stmt.name.lexeme);
            if (stmt.initializer != null) {
                AddStr(" = ");
                stmt.initializer.Accept(this);
                AddStr(";");
                NewLine();
            }
        }

        public override void Visit(WhileStmt stmt) {
            if (isNewLine) {
                isNewLine = false;
                AddStr(WS());
            }
            AddStr("while(");
            stmt.condition.Accept(this);
            AddStr(") ");
            stmt.body.Accept(this);
        }
    }
}
