﻿using System;
using System.Collections.Generic;
using System.Text;

using AtomScript.Runtime;

namespace AtomScript {
    class CompileResult {
        public bool success;
        public Chunk chunk;
        public string message;

        public CompileResult(bool success, Chunk chunk, string message) {
            this.success = success;
            this.chunk = chunk;
            this.message = message;
        }
    }
}
