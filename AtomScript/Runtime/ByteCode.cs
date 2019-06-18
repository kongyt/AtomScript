using System;
using System.Collections.Generic;
using System.Text;


namespace AtomScript.Runtime {
    class ByteCode {
        public uint magic;                                      // 魔数，用于识别文件格式
        public List<Chunk> chunks;                              // Chunks
    }
}
