using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.AST;
using AtomScript.Runtime;

namespace AtomScript.Compiler {
    class Compiler : AstVisitor{

        public CompileResult Compile(Ast ast) {
            Chunk chunk = new Chunk();
            return new CompileResult(true, chunk, null);
        }
    }
}
