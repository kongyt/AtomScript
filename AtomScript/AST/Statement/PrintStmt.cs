using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class PrintStmt : Stmt {
        public Expr expr;

        public PrintStmt(Expr expr) {
            this.expr = expr;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}