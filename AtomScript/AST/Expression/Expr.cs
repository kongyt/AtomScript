namespace AtomScript.AST.Expression {

    abstract class Expr : IVisitable {

        public virtual void Accept(ASTVisitor visitor) {

        }
    }

}