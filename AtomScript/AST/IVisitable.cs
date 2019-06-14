namespace AtomScript.AST {

    interface IVisitable {
        void Accept(ASTVisitor visitor);
    }

}
