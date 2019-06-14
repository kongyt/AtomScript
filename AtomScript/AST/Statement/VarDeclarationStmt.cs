using AtomScript.Scanner;
using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class VarDeclarationStmt : Stmt {
        public Token name;
        public Expr initializer;

        public VarDeclarationStmt(Token name, Expr initializer) {
            this.name = name;
            this.initializer = initializer;
        }

        public override void Accept(ASTVisitor visitor) {
            visitor.Visit(this);
        }
    }

}