using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    // 虚拟机
    class VM {
        public Stack stack;

        public void Init() {

        }

        public void Destroy() {

        }

        public ExecuteResult Execute(Chunk chunk) {
            return new ExecuteResult(true, null);
        }

    }
}
