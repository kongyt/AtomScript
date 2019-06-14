namespace AtomScript.AST {

    interface IVisitable {
        void Accept(AstVisitor visitor);
    }

}
