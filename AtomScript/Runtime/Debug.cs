using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {

    class ChunkDebug {
        public static void Dump(string name, Chunk chunk) {
            Console.WriteLine("== " + name + " ==");
            for (int offset = 0; offset < chunk.count;) {
                offset = DumpInstruction(chunk, offset);
            }
        }

        public static int DumpInstruction(Chunk chunk, int offset) {
            Console.Write(String.Format("{0:D4} ", offset));

            OpCode instruction = (OpCode)chunk.code[offset];
            switch (instruction) {
                case OpCode.RETURN:
                    return SimpleInstruction("OP_RETURN", offset);
                case OpCode.CONSTANT:
                    return ConstantInstruction("OP_CONSTANT", chunk, offset);
                default:
                    return UnkownInstruction(instruction, offset);
            }
        }

        public static int UnkownInstruction(OpCode instruction, int offset) {
            Console.WriteLine("Unknown opcode " + instruction);
            return offset + 1;
        }

        public static int SimpleInstruction(string name, int offset) {
            Console.WriteLine(name);
            return offset + 1;
        }

        public static int ConstantInstruction(string name, Chunk chunk, int offset) {
            Value constant = chunk.ReadConstant(chunk.ReadWord(offset + 1));
            Console.WriteLine(String.Format("{0:-16}-{1:D4} '{2}'", new object[] { name, offset, ValueDebug.FormatValue(constant)}));
            return offset + 3;
        }

    }

    class ClassDebug {

    }

    class EnvironmentDebug {

    }

    class FunctionDebug {

    }

    class ObjDebug {

    }

    class OpCodeDebug {

    }

    class StackDebug {

    }

    class TableDebug<T> {

    }

    class ValueDebug {
        public static string FormatValue(Value value) {
            string res = "Value[" + value.type + " : ";
            switch (value.type) {
                case ValueType.BOOL:
                    res += value.AsBool();
                    break;
                case ValueType.NIL:
                    res += "nil";
                    break;
                case ValueType.NUMBER:
                    res += String.Format("{0:F6}", value.AsNumber());
                    break;
                case ValueType.STRING:
                    res += "\"" + value.AsString() + "\"";
                    break;
                case ValueType.OBJECT:
                    res += "Object";
                    break;
            }
            res += "]";
            return res;
        }
    }

    class VMDebug {

    }
}
