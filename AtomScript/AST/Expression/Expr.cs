namespace AtomScript.AST.Expression {

    abstract class Expr : IVisitable {

        public virtual void Accept(AstVisitor visitor) {

        }
    }

}