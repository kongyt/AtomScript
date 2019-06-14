using AtomScript.Scanner;

namespace AtomScript.AST.Expression {

    class BinaryExpr : Expr {
        public Expr left;
        public Token op;
        public Expr right;

        public BinaryExpr(Expr left, Token op, Expr right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override void Accept(ASTVisitor visitor) {
            visitor.Visit(this);
        }
    }

}