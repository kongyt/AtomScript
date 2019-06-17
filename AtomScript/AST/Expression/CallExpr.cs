using System.Collections.Generic;

namespace AtomScript.AST.Expression {

    class CallExpr : Expr {
        public Expr callee;
        public Token paren;
        public List<Expr> arguments;

        public CallExpr(Expr callee, Token paren, List<Expr> arguments) {
            this.callee = callee;
            this.paren = paren;
            this.arguments = arguments;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}