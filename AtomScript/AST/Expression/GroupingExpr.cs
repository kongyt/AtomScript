namespace AtomScript.AST.Expression {

    class GroupingExpr : Expr {
        public Expr expression;

        public GroupingExpr(Expr expression) {
            this.expression = expression;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}