using System.Collections.Generic;
using AtomScript.AST.Expression;
using AtomScript.AST.Statement;

namespace AtomScript.Parser {

    class ParseResult {
        public bool success;
        public List<Stmt> statements;
        public List<SyntaxError> errors;

        public ParseResult(bool success, List<Stmt> statements, List<SyntaxError> errors) {
            this.success = success;
            this.statements = statements;
            this.errors = errors;
        }

    }

}