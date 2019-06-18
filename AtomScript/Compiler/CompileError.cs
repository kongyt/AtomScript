using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Compiler {
    class CompileError {
        public int line;
        public int column;
        public string message;

        public CompileError(int line, int column, string message) {
            this.line = line;
            this.column = column;
            this.message = message;
        }
    }
}
