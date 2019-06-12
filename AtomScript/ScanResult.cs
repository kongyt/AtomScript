using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {
    class ScanError {
        public int line;
        public int column;
        public string errorStr;

        public ScanError(int line, int column, string errorStr) {
            this.line = line;
            this.column = column;
            this.errorStr = errorStr;
        }
    }

    class ScanResult {
        public bool success;
        public List<ScanError> errors;
        public List<Token> tokens;
    }
}
