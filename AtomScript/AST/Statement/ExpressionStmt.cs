using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class ExpressionStmt : Stmt {
        public Expr expr;

        public ExpressionStmt(Expr expr) {
            this.expr = expr;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}