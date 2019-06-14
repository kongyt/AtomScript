using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class IfStmt : Stmt {
        public Expr condition;
        public Stmt thenBranch;
        public Stmt elseBranch;

        public IfStmt(Expr condition, Stmt thenBranch, Stmt elseBranch) {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}