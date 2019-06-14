using AtomScript.AST.Statement;

namespace AtomScript.AST {

    class Ast : IVisitable {
        public Stmt root;

        public Ast(Stmt root) {
            this.root = root;
        }

        public void Accept(AstVisitor visitor) {
            root.Accept(visitor);
        }
    }

}