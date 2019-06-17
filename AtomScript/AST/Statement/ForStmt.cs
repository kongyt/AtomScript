using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class ForStmt : Stmt {
        public Stmt initializer;
        public Expr condition;
        public Expr increment;
        public Stmt body;

        public ForStmt(Stmt initializer, Expr condition, Expr increment, Stmt body) {
            this.initializer = initializer;
            this.condition = condition;
            this.increment = increment;
            this.body = body;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}