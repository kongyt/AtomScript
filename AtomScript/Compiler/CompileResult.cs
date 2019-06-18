using System;
using System.Collections.Generic;

using AtomScript.Runtime;

namespace AtomScript.Compiler {
    class CompileResult {
        public bool success;
        public Chunk chunk;
        public List<CompileError> errors;

        public CompileResult(bool success, Chunk chunk, List<CompileError> errors) {
            this.success = success;
            this.chunk = chunk;
            this.errors = errors;
        }
    }
}
