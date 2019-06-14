using AtomScript.AST.Expression;
using AtomScript.AST.Statement;

namespace AtomScript.AST {

    abstract class AstVisitor {

        public virtual void Visit(UnaryExpr expr) {

        }

        public virtual void Visit(BinaryExpr expr) {

        }

        public virtual void Visit(LiteralExpr expr) {

        }

        public virtual void Visit(GroupingExpr expr) {

        }

        public virtual void Visit(LogicalExpr expr) {

        }

        public virtual void Visit(VariableExpr expr) {

        }

        public virtual void Visit(AssignExpr expr) {

        }

        public virtual void Visit(ExpressionStmt stmt) {

        }

        public virtual void Visit(IfStmt stmt) {

        }

        public virtual void Visit(PrintStmt stmt) {

        }

        public virtual void Visit(WhileStmt stmt) {

        }

        public virtual void Visit(VarDeclarationStmt stmt) {

        }

        public virtual void Visit(BlockStmt stmt) {

        }
    }

}