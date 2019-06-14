using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class WhileStmt : Stmt {
        public Expr condition;
        public Stmt body;

        public WhileStmt(Expr condition, Stmt body) {
            this.condition = condition;
            this.body = body;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}