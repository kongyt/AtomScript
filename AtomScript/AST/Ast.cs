using System.Collections.Generic;
using AtomScript.AST.Statement;

namespace AtomScript.AST {

    class Ast : IVisitable {
        public List<Stmt> stmts;

        public Ast(List<Stmt> stmts) {
            this.stmts = stmts;
        }

        public void Accept(AstVisitor visitor) {
            for (int i = 0; i < stmts.Count; i++) {
                stmts[i].Accept(visitor);
            }
        }
    }

}