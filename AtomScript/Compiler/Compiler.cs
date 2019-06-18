using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.AST;
using AtomScript.AST.Expression;
using AtomScript.AST.Statement;

using AtomScript.Runtime;

namespace AtomScript.Compiler {
    class Compiler : AstVisitor{
        private bool success;
        private Chunk chunk;
        private List<CompileError> errors;

        private void Reset() {
            success = true;
            chunk = new Chunk();
            errors = new List<CompileError>();
        }

        private void ReportError(int line, int column, string message) {
            success = false;
            this.errors.Add(new CompileError(line, column, message));
        }

        public CompileResult Compile(Ast ast) {
            Reset();
            ast.Accept(this);

            //chunk.WriteOpCode(OpCode.RETURN);
            //chunk.WriteOpCode(OpCode.CONSTANT);
            //chunk.WriteWord(chunk.AddConstant(new Value(1)));
            //chunk.WriteOpCode(OpCode.NEGATE);
            //chunk.WriteOpCode(OpCode.CONSTANT);
            //chunk.WriteWord(chunk.AddConstant(new Value(123)));
            //chunk.WriteOpCode(OpCode.DIV);
            //chunk.WriteOpCode(OpCode.CONSTANT);
            //chunk.WriteWord(chunk.AddConstant(new Value(1)));
            //chunk.WriteOpCode(OpCode.ADD);
            //chunk.WriteOpCode(OpCode.CONSTANT);
            //chunk.WriteWord(chunk.AddConstant(new Value(0.1)));
            //chunk.WriteOpCode(OpCode.DIV);
            //ChunkDebug.Dump("Chunk Dump:", chunk);

            return new CompileResult(success, chunk, errors);
        }

        public override void Visit(AssignExpr expr) {
            expr.value.Accept(this);
            ushort addr = chunk.AddConstant(new Value(expr.name.lexeme));
            chunk.WriteOpCode(OpCode.SET_GLOBAL);
            chunk.WriteWord(addr);
        }

        public override void Visit(BinaryExpr expr) {
            expr.left.Accept(this);            
            expr.right.Accept(this);

            switch (expr.op.type) {
                case TokenType.PLUS:
                    chunk.WriteOpCode(OpCode.ADD);
                    break;
                case TokenType.MINUS:
                    chunk.WriteOpCode(OpCode.SUB);
                    break;
                case TokenType.STAR:
                    chunk.WriteOpCode(OpCode.MUL);
                    break;
                case TokenType.SLASH:
                    chunk.WriteOpCode(OpCode.DIV);
                    break;
                case TokenType.EQUAL_EQUAL:
                    chunk.WriteOpCode(OpCode.EQUAL);
                    break;
                case TokenType.BANG_EQUAL:
                    chunk.WriteOpCode(OpCode.NOT_EQUAL);
                    break;
                case TokenType.GREATER:
                    chunk.WriteOpCode(OpCode.GREATER);
                    break;
                case TokenType.GREATER_EQUAL:
                    chunk.WriteOpCode(OpCode.GREATER_EQUAL);
                    break;
                case TokenType.LESS:
                    chunk.WriteOpCode(OpCode.LESS);
                    break;
                case TokenType.LESS_EQUAL:
                    chunk.WriteOpCode(OpCode.LESS_EQUAL);
                    break;

            }
        }

        public override void Visit(CallExpr expr) {
            expr.callee.Accept(this);
        }

        public override void Visit(GetExpr expr) {
            expr.obj.Accept(this);
        }

        public override void Visit(GroupingExpr expr) {
            expr.expression.Accept(this);
        }

        public override void Visit(LiteralExpr expr) {
            Value value;
            switch (expr.literal.type) {
                case TokenType.TRUE:
                    chunk.WriteOpCode(OpCode.TRUE);
                    break;
                case TokenType.FALSE:
                    chunk.WriteOpCode(OpCode.FALSE);
                    break;
                case TokenType.NIL:
                    chunk.WriteOpCode(OpCode.NIL);
                    break;
                case TokenType.NUMBER:
                    value = new Value((double)expr.literal.literal);
                    AddConstant(value);
                    break;
                case TokenType.STRING:
                    value = new Value((string)expr.literal.literal);
                    AddConstant(value);
                    break;
                default:
                    value = new Value();
                    AddConstant(value);
                    ReportError(expr.literal.line, expr.literal.column, "Unknown literal.");                    
                    break;
            }            
        }



        public override void Visit(LogicalExpr expr) {
            expr.left.Accept(this);
            expr.right.Accept(this);
        }

        public override void Visit(SetExpr expr) {
            expr.obj.Accept(this);
            expr.value.Accept(this);
        }

        public override void Visit(SuperExpr expr) {
            
        }

        public override void Visit(ThisExpr expr) {
            
        }

        public override void Visit(UnaryExpr expr) {
            expr.right.Accept(this);
            switch (expr.op.type) {
                case TokenType.MINUS:
                    chunk.WriteOpCode(OpCode.NEGATE);
                    break;
                case TokenType.BANG:
                    // TODO
                    break;
            }
        }

        public override void Visit(VariableExpr expr) {
            ushort addr = chunk.AddConstant(new Value(expr.name.lexeme));
            chunk.WriteOpCode(OpCode.GET_GLOBAL);
            chunk.WriteWord(addr);
        }

        public override void Visit(BlockStmt stmt) {
            for (int i = 0; i < stmt.stmts.Count; i++) {
                stmt.stmts[i].Accept(this);
            }
            
        }

        public override void Visit(ClassDeclarationStmt stmt) {
            
        }

        public override void Visit(ExpressionStmt stmt) {
            stmt.expr.Accept(this);
        }

        public override void Visit(ForStmt stmt) {

        }

        public override void Visit(FuncDeclarationStmt stmt) {

        }

        public override void Visit(IfStmt stmt) {

        }

        public override void Visit(PrintStmt stmt) {
            stmt.expr.Accept(this);
            chunk.WriteOpCode(OpCode.PRINT);            
        }

        public override void Visit(ReturnStmt stmt) {

        }

        public override void Visit(VarDeclarationStmt stmt) {
            if (stmt.initializer != null) {
                stmt.initializer.Accept(this);
            } else {
                chunk.WriteOpCode(OpCode.NIL);
            }
            ushort addr = chunk.AddConstant(new Value(stmt.name.lexeme));
            chunk.WriteOpCode(OpCode.DEF_GLOBAL);
            chunk.WriteWord(addr);
        }

        public override void Visit(WhileStmt stmt) {

        }

        private void AddConstant(Value value) {
            ushort addr = chunk.AddConstant(value);
            chunk.WriteOpCode(OpCode.CONSTANT);
            chunk.WriteWord(addr);
        }
    }
}
