namespace AtomScript.AST.Expression {

    class GetExpr : Expr {
        public Expr obj;
        public Token name;

        public GetExpr(Expr obj, Token name) {
            this.obj = obj;
            this.name = name;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}