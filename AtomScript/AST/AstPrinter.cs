using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.AST.Expression;
using AtomScript.AST.Statement;
using AtomScript.Scanner;

namespace AtomScript.AST {
    class AstPrinter : AstVisitor {

        private string astStr;

        public string Print(Ast ast) {
            astStr = "";
            ast.Accept(this);
            Console.WriteLine(astStr);
            return astStr;
        }

        public override void Visit(UnaryExpr expr) {
            astStr += expr.op.lexeme;
            expr.right.Accept(this);
        }

        public override void Visit(BinaryExpr expr) {
            expr.left.Accept(this);
            astStr = astStr + " " + expr.op.lexeme + " ";
            expr.right.Accept(this);
        }

        public override void Visit(LiteralExpr expr) {
            astStr += expr.literal.lexeme;
        }

        public override void Visit(GroupingExpr expr) {
            astStr += "( ";
            expr.expression.Accept(this);
            astStr += " )";
        }

        public override void Visit(LogicalExpr expr) {
            expr.left.Accept(this);
            astStr = astStr + " " + expr.op.lexeme + " ";
            expr.right.Accept(this);
        }

        public override void Visit(VariableExpr expr) {
            astStr += expr.name.lexeme;
        }

        public override void Visit(AssignExpr expr) {
            astStr += expr.name.lexeme;
            astStr += " = ";
            expr.value.Accept(this);
        }

        public override void Visit(ExpressionStmt stmt) {
            stmt.expr.Accept(this);
            astStr += ";\n";
        }

        public override void Visit(IfStmt stmt) {
            astStr += "if (";
            stmt.condition.Accept(this);
            astStr += ") \n";
            stmt.thenBranch.Accept(this);
            if (stmt.elseBranch != null) {
                astStr += "else\n";
                stmt.elseBranch.Accept(this);
            }
        }

        public override void Visit(PrintStmt stmt) {
            astStr = astStr + "print ";
            stmt.expr.Accept(this); 
            astStr += ";\n";
        }

        public override void Visit(WhileStmt stmt) {
            astStr += "while(";
            stmt.condition.Accept(this);
            astStr += ")\n";
            stmt.body.Accept(this);
        }

        public override void Visit(VarDeclarationStmt stmt) {
            astStr += "var " + stmt.name.lexeme;
            if (stmt.initializer != null) {
                astStr += " = ";
                stmt.initializer.Accept(this);
                astStr += ";\n";
            }
        }

        public override void Visit(BlockStmt stmt) {
            astStr += "{\n";
            for (int i = 0; i < stmt.stmts.Count; i++) {
                stmt.stmts[i].Accept(this);
            }
            astStr += "}\n";
        }
    }
}
