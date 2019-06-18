using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    class ExecuteResult {
        public bool success;
        public string message;

        public ExecuteResult(bool success, string message) {
            this.success = success;
            this.message = message;
        }
    }
}
