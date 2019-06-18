using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    enum InterpretResult {
        SUCCESS,
        LEXICAL_ERROR,
        SYNTAX_ERROR,
        COMPILE_ERROR,
        RUNTIME_ERROR
    }
}
