using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.AST.Expression;
using AtomScript.AST.Statement;
using AtomScript.Scanner;

namespace AtomScript.AST {
    class ASTPrinter : ASTVisitor {

        private string astStr;

        public string Print(AST ast) {
            astStr = "";
            ast.Accept(this);
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
            stmt.Accept(this);
            astStr += ";";
        }

        public override void Visit(IfStmt stmt) {

        }

        public override void Visit(PrintStmt stmt) {

        }

        public override void Visit(WhileStmt stmt) {

        }

        public override void Visit(VarDeclarationStmt stmt) {

        }

        public override void Visit(BlockStmt stmt) {

        }
    }
}
