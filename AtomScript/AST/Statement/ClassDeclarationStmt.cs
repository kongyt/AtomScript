using System.Collections.Generic;
using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class ClassDeclarationStmt : Stmt {
        public Token name;
        public VariableExpr superclass;
        public List<FuncDeclarationStmt> methods;

        public ClassDeclarationStmt(Token name, VariableExpr superclass, List<FuncDeclarationStmt> methods) {
            this.name = name;
            this.superclass = superclass;
            this.methods = methods;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}