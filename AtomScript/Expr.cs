using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {
    abstract class Expr {

    }

    class Unary : Expr {
        public Token op;
        public Expr right;

        public Unary(Token op, Expr right) {
            this.op = op;
            this.right = right;
        }

        public override string ToString() {
            return op.lexeme + right;
        }
    }

    class Logical : Expr {
        public Expr left;
        public Token op;
        public Expr right;

        public Logical(Expr left, Token op, Expr right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override string ToString() {
            return left + op.lexeme + right;
        }
    }

    class Binary : Expr {
        public Expr left;
        public Token op;
        public Expr right;

        public Binary(Expr left, Token op, Expr right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }

        public override string ToString() {
            return left + op.lexeme + right;
        }
    }

    class Literal : Expr {
        public object value;

        public Literal(object value) {
            this.value = value;
        }

        public override string ToString() {
            return value.ToString();
        }
    }

    class Grouping : Expr {
        public Expr expression;

        public Grouping(Expr expr) {
            this.expression = expr;
        }

        public override string ToString() {
            return "(" + expression + ")";
        }
    }

    class Variable : Expr {
        public Token name;

        public Variable(Token name) {
            this.name = name;
        }

        public override string ToString() {
            return name.lexeme;
        }
    }

    class Assign : Expr {
        public Token name;
        public Expr value;

        public Assign(Token name, Expr value) {
            this.name = name;
            this.value = value;
        }

        public override string ToString() {
            return name.lexeme + " = " + value.ToString();
        }
    }
}
