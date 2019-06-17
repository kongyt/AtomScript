namespace AtomScript.AST.Expression {

    class ThisExpr : Expr {
        public Token keyword;

        public ThisExpr(Token keyword) {
            this.keyword = keyword;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}