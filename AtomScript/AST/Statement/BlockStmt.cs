using System.Collections.Generic;
using AtomScript.AST.Expression;

namespace AtomScript.AST.Statement {

    class BlockStmt : Stmt {
        public List<Stmt> stmts;

        public BlockStmt(List<Stmt> stmts) {
            this.stmts = stmts;
        }

        public override void Accept(AstVisitor visitor) {
            visitor.Visit(this);
        }
    }

}