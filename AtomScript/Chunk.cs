using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {
    class Chunk {
        public int count;
        public int capacity;
        public byte[] code;
        public ValueTable<double> doubleValue = new ValueTable<double>();

        public void Reset() {
            count = 0;
            capacity = 0;
            code = null;
            doubleValue.Reset();
        }

        public void Write(OpCode opCode) {
            if (capacity < count + 1) {
                GrowCapacity();
            }
            code[count] = (byte)opCode;
            count++;
        }

        public void Write(double value) {

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

        public void Dump(string name) {
            Console.WriteLine("== " + name + " ==");
            for (int offset = 0; offset < count;) {
                offset = DumpInstruction(offset);
            }
        }

        public int DumpInstruction(int offset) {
            Console.Write(String.Format("{0:D4} ", offset));

            OpCode instruction = (OpCode)code[offset];
            switch (instruction) {
                case OpCode.RETURN:
                    return SimpleInstruction("OP_RETURN", offset);
                default:
                    return UnkownInstruction(instruction, offset);
            }
        }

        public int UnkownInstruction(OpCode instruction, int offset) {
            Console.WriteLine("Unknown opcode " + instruction);
            return offset + 1;
        }

        public int SimpleInstruction(string name, int offset) {
            Console.WriteLine(name);
            return offset + 1;
        }

        public int ConstantInstruction(string name, int offset) {
            Console.WriteLine(String.Format("{0:-16}-{1:D4} '{2}'", new object[] { name, offset,  }));
            return offset + 2;
        }

    }
}
