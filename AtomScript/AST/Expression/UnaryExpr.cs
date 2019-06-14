using AtomScript.Scanner;

namespace AtomScript.AST.Expression {

    class UnaryExpr : Expr {
        public Token op;
        public Expr right;

        public UnaryExpr(Token op, Expr right) {
            this.op = op;
            this.right = right;
        }

        public override void Accept(ASTVisitor visitor) {
            visitor.Visit(this);
        }
    }

}