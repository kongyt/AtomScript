using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {

    class ChunkDebug {
        public static void Dump(string name, Chunk chunk) {
            Console.WriteLine("== " + name + " ==");
            if (chunk == null) {
                Console.WriteLine("    No Chunk.");
                return;
            }
            for (int offset = 0; offset < chunk.count;) {
                offset = DumpInstruction(chunk, offset);
            }
        }

        public static int DumpInstruction(Chunk chunk, int offset) {
            Console.Write(String.Format("{0:X4} ", offset));

            OpCode instruction = (OpCode)chunk.code[offset];
            switch (instruction) {
                case OpCode.RETURN:
                    return SimpleInstruction("OP_RETURN", offset);
                case OpCode.CONSTANT:
                    return ConstantInstruction("OP_CONSTANT", chunk, offset);
                case OpCode.NIL:
                    return SimpleInstruction("OP_NIL", offset);
                case OpCode.TRUE:
                    return SimpleInstruction("OP_TRUE", offset);
                case OpCode.FALSE:
                    return SimpleInstruction("OP_FALSE", offset);
                case OpCode.DEF_GLOBAL:
                    return ConstantInstruction("OP_DEF_GLOBAL", chunk, offset);
                case OpCode.GET_GLOBAL:
                    return ConstantInstruction("OP_GET_GLOBAL", chunk, offset);
                case OpCode.SET_GLOBAL:
                    return ConstantInstruction("OP_SET_GLOBAL", chunk, offset);
                case OpCode.NEGATE:
                    return SimpleInstruction("OP_NEGATE", offset);
                case OpCode.ADD:
                    return SimpleInstruction("OP_ADD", offset);
                case OpCode.SUB:
                    return SimpleInstruction("OP_SUB", offset);
                case OpCode.MUL:
                    return SimpleInstruction("OP_MUL", offset);
                case OpCode.DIV:
                    return SimpleInstruction("OP_DIV", offset);
                case OpCode.PRINT:
                    return SimpleInstruction("OP_PRINT", offset);
                case OpCode.EQUAL:
                    return SimpleInstruction("OP_EQUAL", offset);
                case OpCode.NOT_EQUAL:
                    return SimpleInstruction("OP_NOT_EQUAL", offset);
                case OpCode.GREATER:
                    return SimpleInstruction("OP_GREATER", offset);
                case OpCode.GREATER_EQUAL:
                    return SimpleInstruction("OP_GREATER_EQUAL", offset);
                case OpCode.LESS:
                    return SimpleInstruction("OP_LESS", offset);
                case OpCode.LESS_EQUAL:
                    return SimpleInstruction("OP_LESS_EQUAL", offset);
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
            ushort constantAddr = chunk.ReadWord(offset + 1);
            Value constant = chunk.ReadConstant(constantAddr);
            Console.WriteLine(String.Format("{0:-16}    {1}", new object[] { name, ValueDebug.FormatValue(constant) }));
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
        public static void Dump(string name, VM vm) {
            Console.WriteLine("== " + name + " ==");
            if (vm == null) return;
            Console.WriteLine("Stack:");
            if (vm.stackTop == 0) {
                Console.WriteLine("    Enpty.");
            } else {
                for (int i = 0; i < vm.stackTop; i++) {
                    Console.WriteLine("    " + ValueDebug.FormatValue(vm.stack[i]));
                }
            }                      
        }
    }
}
