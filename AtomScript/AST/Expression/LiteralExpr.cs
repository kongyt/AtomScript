using AtomScript.Scanner;

namespace AtomScript.AST.Expression {

    class LiteralExpr : Expr {
        public Token literal;

        public LiteralExpr(Token literal) {
            this.literal = literal;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}