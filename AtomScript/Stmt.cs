using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {
    abstract class Stmt {

    }

    class Expression : Stmt {
        public Expr expr;

        public Expression(Expr expr) {
            this.expr = expr;
        }

        public override string ToString() {
            return expr.ToString() + ";";
        }
    }

    class If : Stmt{
        public Expr condition;
        public Stmt thenBranch;
        public Stmt elseBranch;

        public If(Expr condition, Stmt thenBranch, Stmt elseBranch) {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }

        public override string ToString() {
            string str = "if ( " + condition.ToString() + " ) \n";
            str = str + thenBranch.ToString() + "\n";
            if (elseBranch != null) {
                str += "else\n";
                str = str + elseBranch.ToString() + "\n";
            }
            return str;
        }
    }

    class Print : Stmt {
        public Expr expr;

        public Print(Expr expr) {
            this.expr = expr;
        }

        public override string ToString() {
            return "print " + expr.ToString() + ";";
        }
    }

    class Var : Stmt {
        public Token name;
        public Expr initializer;

        public Var(Token name, Expr initializer) {
            this.name = name;
            this.initializer = initializer;
        }

        public override string ToString() {
            if (initializer == null) {
                return "var " + name.lexeme + ";";
            } else {
                return "var " + name.lexeme + " = " + this.initializer.ToString() + ";";
            }            
        }
    }

    class Block : Stmt {
        public List<Stmt> statements;

        public Block(List<Stmt> stats) {
            this.statements = stats;
        }

        public override string ToString() {
            string str = "{";
            for (int i = 0; i < statements.Count; i++) {
                str += "\n" + (statements[i].ToString());
            }
            str += "\n}";
            return str;
        }
    }
}
