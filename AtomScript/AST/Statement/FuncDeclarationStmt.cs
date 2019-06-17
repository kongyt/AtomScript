using System.Collections.Generic;

namespace AtomScript.AST.Statement {

    class FuncDeclarationStmt : Stmt {
        public Token name;
        public List<Token> parameters;
        public BlockStmt body;

        public FuncDeclarationStmt(Token name, List<Token> parameters, BlockStmt body) {
            this.name = name;
            this.parameters = parameters;
            this.body = body;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}