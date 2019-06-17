using AtomScript.AST.Expression;
using AtomScript.AST.Statement;

namespace AtomScript.AST {

    abstract class AstVisitor {

        public virtual void Visit(AssignExpr expr) {

        }

        public virtual void Visit(BinaryExpr expr) {

        }

        public virtual void Visit(CallExpr expr) {

        }

        public virtual void Visit(GetExpr expr) {

        }

        public virtual void Visit(GroupingExpr expr) {

        }

        public virtual void Visit(LiteralExpr expr) {

        }

        public virtual void Visit(LogicalExpr expr) {

        }

        public virtual void Visit(SetExpr expr) {

        }

        public virtual void Visit(SuperExpr expr) {

        }

        public virtual void Visit(ThisExpr expr) {

        }

        public virtual void Visit(UnaryExpr expr) {

        }

        public virtual void Visit(VariableExpr expr) {

        }

        public virtual void Visit(BlockStmt stmt) {

        }

        public virtual void Visit(ClassDeclarationStmt stmt) {

        }

        public virtual void Visit(ExpressionStmt stmt) {

        }

        public virtual void Visit(ForStmt stmt) {

        }

        public virtual void Visit(FuncDeclarationStmt stmt) {

        }

        public virtual void Visit(IfStmt stmt) {

        }

        public virtual void Visit(PrintStmt stmt) {

        }

        public virtual void Visit(ReturnStmt stmt) {

        }

        public virtual void Visit(VarDeclarationStmt stmt) {

        }

        public virtual void Visit(WhileStmt stmt) {

        }
    }

}