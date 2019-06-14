using System.Collections.Generic;

namespace AtomScript.Scanner {

    class ScanResult {
        public bool success;
        public List<Token> tokens;
        public List<LexicalError> errors;

        public ScanResult(bool success, List<Token> tokens, List<LexicalError> errors) {
            this.success = success;
            this.tokens = tokens;
            this.errors = errors;
        }

    }

}