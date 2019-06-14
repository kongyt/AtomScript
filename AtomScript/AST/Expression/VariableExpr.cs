using AtomScript.Scanner;

namespace AtomScript.AST.Expression {

    class VariableExpr : Expr {
        public Token name;

        public VariableExpr(Token name) {
            this.name = name;
        }

        public override void Accept(ASTVisitor visitor) {
            visitor.Visit(this);
        }
    }

}