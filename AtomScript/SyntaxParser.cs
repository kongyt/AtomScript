using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {

    class SyntaxError {
        public int line;
        public int column;
        public string errorStr;
    }

    class SyntaxResult {
        public bool success;
        public List<SyntaxError> errors;
    }

    class SyntaxParser {

    }
}
