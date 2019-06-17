namespace AtomScript.AST.Expression {

    class SetExpr : Expr {
        public Expr obj;
        public Token name;
        public Expr value;

        public SetExpr(Expr obj, Token name, Expr value) {
            this.obj = obj;
            this.name = name;
            this.value = value;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}