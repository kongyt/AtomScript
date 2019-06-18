using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    class Function : Obj{
        public string name;
        public List<Obj> arguments;

        public int Arity() {
            return arguments.Count;
        }

        public void Call(Environment env, List<Obj> args) {

        }
    }
}
