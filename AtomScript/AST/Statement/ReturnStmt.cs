using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class ReturnStmt : Stmt {
        public Token keyword;
        public Expr value;

        public ReturnStmt(Token keyword, Expr value) {
            this.keyword = keyword;
            this.value = value;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}