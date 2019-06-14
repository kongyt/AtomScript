namespace AtomScript.AST.Statement {

    abstract class Stmt : IVisitable {

        public virtual void Accept(ASTVisitor visitor) {

        }
    }

}