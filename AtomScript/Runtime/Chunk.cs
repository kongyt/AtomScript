using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    
    class Chunk {
        public Table<Value> constantTable = new Table<Value>(); // 常量表，地址为双字节，要求表的数量最多不超过65535个
        public int count;
        public int capacity;
        public byte[] code;                                     // 代码

        public Chunk() {
            count = 0;
            capacity = 4;
            code = new byte[4];
        }
        public Chunk(int capacity) {
            count = 0;
            this.capacity = capacity;
            code = new byte[capacity];
        }

        private void CheckCapacity() {
            if (capacity < count + 1) {
                GrowCapacity();
            }
        }

        private void GrowCapacity() {
            int oldCapacity = capacity;
            int newCapacity = capacity < 8 ? 8 : (capacity * 2);

            byte[] buf = new byte[newCapacity];
            if (oldCapacity > 0) {
                Buffer.BlockCopy(code, 0, buf, 0, oldCapacity);
            }
            code = buf;
            capacity = newCapacity;
        }

        public void WriteByte(byte data) {
            CheckCapacity();
            code[count] = data;
            count++;
        }

        public byte ReadByte(int offset) {
            return code[offset];
        }

        public void WriteWord(ushort constant) {
            WriteByte((byte)constant);
            WriteByte((byte)(constant >> 8));
        }

        public ushort ReadWord(int offset) {
            int low = (int)ReadByte(offset);
            int high = (int)ReadByte(offset + 1) << 8;
            return (ushort)(low | high);
        }

        public void WriteOpCode(OpCode opCode) {
            ReadByte((byte)opCode);
        }

        public OpCode ReadOpCode(int offset) {
            return (OpCode)ReadByte(offset);
        }

        public bool AddConstant(Value constant, out ushort addr) {
            if (constantTable.count >= ushort.MaxValue) {
                addr = 0x0;
                return false;
            }
            addr = (ushort)constantTable.Add(constant);
            return true;
        }

        public Value ReadConstant(ushort addr) {
            return constantTable.data[addr];
        }

    }
}
