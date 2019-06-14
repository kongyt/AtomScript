using AtomScript.AST.Statement;

namespace AtomScript.AST {

    class AST : IVisitable {
        public Stmt root;

        public AST(Stmt root) {
            this.root = root;
        }

        public void Accept(ASTVisitor visitor) {
            root.Accept(visitor);
        }
    }

}