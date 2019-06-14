using System.Collections.Generic;
using AtomScript.AST;

namespace AtomScript.Parser {

    class ParseResult {
        public bool success;
        public Ast ast;
        public List<SyntaxError> errors;

        public ParseResult(bool success, Ast ast, List<SyntaxError> errors) {
            this.success = success;
            this.ast = ast;
            this.errors = errors;
        }

    }

}