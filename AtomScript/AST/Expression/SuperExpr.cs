namespace AtomScript.AST.Expression {

    class SuperExpr : Expr {
        public Token keyword;
        public Token method;

        public SuperExpr(Token keyword, Token method) {
            this.keyword = keyword;
            this.method = method;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}