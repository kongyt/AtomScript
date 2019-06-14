using AtomScript.Scanner;

namespace AtomScript.AST.Expression {

    class AssignExpr : Expr {
        public Token name;
        public Expr value;

        public AssignExpr(Token name, Expr value) {
            this.name = name;
            this.value = value;
        }

        public override void Accept(ASTVisitor visitor) {
            visitor.Visit(this);
        }
    }

}