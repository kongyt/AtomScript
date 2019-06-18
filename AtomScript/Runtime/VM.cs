using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    // 虚拟机
    class VM {
        public Chunk chunk;
        public int pc;
        public Value[] stack;
        public int stackTop;
        public Dictionary<string, Value> globalVars;

        public bool success;
        public string message;

        public void Init() {
            chunk = null;
            pc = 0;
            stack = new Value[256];
            stackTop = 0;
            globalVars = new Dictionary<string, Value>();
        }

        public void Destroy() {

        }

        public ExecuteResult Execute(Chunk chunk) {
            success = true;
            message = null;
            this.chunk = chunk;
            pc = 0;
            while (pc < this.chunk.count && success) {
                OpCode op = this.chunk.ReadOpCode(pc);
                switch (op) {
                    case OpCode.RETURN:
                        OpReturn();
                        break;
                    case OpCode.CONSTANT:
                        OpConstant();
                        break;
                    case OpCode.NIL:
                        OpNil();
                        break;
                    case OpCode.TRUE:
                        OpTrue();
                        break;
                    case OpCode.FALSE:
                        OpFalse();
                        break;
                    case OpCode.DEF_GLOBAL:
                        OpDefGlobal();
                        break;
                    case OpCode.GET_GLOBAL:
                        OpGetGlobal();
                        break;
                    case OpCode.SET_GLOBAL:
                        OpSetGlobal();
                        break;
                    case OpCode.NEGATE:
                        OpNegate();
                        break;
                    case OpCode.ADD:
                        OpAdd();
                        break;
                    case OpCode.SUB:
                        OpSub();
                        break;
                    case OpCode.MUL:
                        OpMul();
                        break;
                    case OpCode.DIV:
                        OpDiv();
                        break;
                    case OpCode.PRINT:
                        OpPrint();
                        break;
                    case OpCode.EQUAL:
                        OpEqual();
                        break;
                    case OpCode.NOT_EQUAL:
                        OpNotEqual();
                        break;
                    case OpCode.GREATER:
                        OpGreator();
                        break;
                    case OpCode.GREATER_EQUAL:
                        OpGreatorEqual();
                        break;
                    case OpCode.LESS:
                        OpLess();
                        break;
                    case OpCode.LESS_EQUAL:
                        OpLessEqual();
                        break;
                    default:
                        OpUnknown(op);
                        break;
                }
                //Debug();
            }

            return new ExecuteResult(success, message);
        }

        public void Debug() {
            Console.WriteLine();
            VMDebug.Dump("VM Dump", this);
            Console.WriteLine("");
            ChunkDebug.Dump("ChunkDump", this.chunk);
            Console.WriteLine("");
        }

        private void OpReturn() {
            Advance(1);
        }

        private void OpConstant() {
            Advance(1);
            ushort addr = this.chunk.ReadWord(pc);
            Advance(2);
            Push(chunk.ReadConstant(addr));
        }

        private void OpNil() {
            Advance(1);
            Push(new Value());
        }

        private void OpTrue() {
            Advance(1);
            Push(new Value(true));
        }

        private void OpFalse() {
            Advance(1);
            Push(new Value(false));
        }

        private void OpDefGlobal() {
            Advance(1);
            ushort addr = this.chunk.ReadWord(pc);
            Advance(2);
            Value nameConstant = chunk.ReadConstant(addr);
            if (nameConstant.type == ValueType.STRING) {
                string varName = nameConstant.AsString();
                globalVars.Add(varName, Pop());
            } else {
                ReportError("TypeError: bad global var name type :" + nameConstant.type);
            }            
        }

        private void OpGetGlobal() {
            Advance(1);
            ushort addr = this.chunk.ReadWord(pc);
            Advance(2);
            Value nameConstant = chunk.ReadConstant(addr);
            if (nameConstant.type == ValueType.STRING) {
                string varName = nameConstant.AsString();
                Value var;
                if (globalVars.TryGetValue(varName, out var)) {
                    Push(new Value(var));
                } else {
                    ReportError("NameError: name \'" + varName + "\' is not defined.");
                }
            } else {
                ReportError("TypeError: bad global var name type :" + nameConstant.type);
            }
        }

        private void OpSetGlobal() {
            Advance(1);
            ushort addr = this.chunk.ReadWord(pc);
            Advance(2);
            Value nameConstant = chunk.ReadConstant(addr);
            if (nameConstant.type == ValueType.STRING) {
                string varName = nameConstant.AsString();
                if (globalVars.ContainsKey(varName)) {
                    globalVars[varName] = Pop();
                } else {
                    ReportError("NameError: name \'" + varName + "\' is not defined.");
                }
            } else {
                ReportError("TypeError: bad global var name type :" + nameConstant.type);
            }
        }

        private void OpNegate() {
            Advance(1);
            Value v1 = Pop();
            if (v1.type == ValueType.NUMBER) {
                v1.data = -v1.AsNumber();
                Push(v1);
            } else {
                ReportError("TypeError: bad operand type(" + v1.type + ") for unary \'-\'.");
            }
        }

        private void OpAdd() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                v2.data = v2.AsNumber() + v1.AsNumber();
                Push(v2);
            } else if (v2.type == ValueType.STRING && v1.type == ValueType.STRING) {
                v2.data = v2.AsString() + v1.AsString();
                Push(v2);
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'+\'.");
            }         
        }

        private void OpSub() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                v2.data = v2.AsNumber() - v1.AsNumber();
                Push(v2);
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'-\'.");
            }
        }

        private void OpMul() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                v2.data = v2.AsNumber() * v1.AsNumber();
                Push(v2);
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'*\'.");
            }
        }

        private void OpDiv() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                double temp = v1.AsNumber();
                if (Math.Abs(temp) < 0.000001) {
                    ReportError("ZeroDivisionError: division by zero.");
                } else {
                    v2.data = v2.AsNumber() / v1.AsNumber();
                    Push(v2);
                }
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'/\'.");
            }
        }

        private void OpPrint() {
            Advance(1);
            Value value = Pop();
            if (value == null) {
                return;
            }
            string str = "";
            switch (value.type) {
                case ValueType.BOOL:
                    str += value.AsBool();
                    break;
                case ValueType.NIL:
                    str += "nil";
                    break;
                case ValueType.NUMBER:
                    str += String.Format("{0:F6}", value.AsNumber());
                    break;
                case ValueType.STRING:
                    str += "\"" + value.AsString() + "\"";
                    break;
                case ValueType.OBJECT:
                    str += "Object";
                    break;
            }
            Console.WriteLine(str);
        }

        private void OpEqual() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(Math.Abs(v2.AsNumber() - v1.AsNumber()) < 0.000001));
            } else if (v2.type == ValueType.STRING && v2.type == ValueType.STRING) {
                Push(new Value(String.Equals(v2.AsString(), v1.AsString())));
            } else if (v2.type == ValueType.BOOL && v1.type == ValueType.BOOL) {
                Push(new Value(v2.AsBool() == v1.AsBool()));
            } else if (v2.type == ValueType.NIL && v1.type == ValueType.NIL) {
                Push(new Value(true));
            } else {
                Push(new Value(false));
            }
        }

        private void OpNotEqual() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(!(Math.Abs(v2.AsNumber() - v1.AsNumber()) < 0.000001)));
            } else if (v2.type == ValueType.STRING && v2.type == ValueType.STRING) {
                Push(new Value(!String.Equals(v2.AsString(), v1.AsString())));
            } else if (v2.type == ValueType.BOOL && v1.type == ValueType.BOOL) {
                Push(new Value(v2.AsBool() != v1.AsBool()));
            } else if (v2.type == ValueType.NIL && v1.type == ValueType.NIL) {
                Push(new Value(false));
            } else {
                Push(new Value(true));
            }
        }

        private void OpGreator() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(v2.AsNumber() > v1.AsNumber()));
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'>\'.");
            }
        }
        private void OpGreatorEqual() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(v2.AsNumber() >= v1.AsNumber()));
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'>=\'.");
            }
        }
        private void OpLess() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(v2.AsNumber() < v1.AsNumber()));
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'<\'.");
            }
        }
        private void OpLessEqual() {
            Advance(1);
            Value v1 = Pop();
            Value v2 = Pop();
            if (v2.type == ValueType.NUMBER && v1.type == ValueType.NUMBER) {
                Push(new Value(v2.AsNumber() <= v1.AsNumber()));
            } else {
                ReportError("TypeError: bad operand type(" + v2.type + ") for binary \'<=\'.");
            }
        }

        private void OpUnknown(OpCode op) {
            ReportError("Unknown opcode (" + op + ").");
            Advance(1);            
        }

        public void Advance(int count) {
            this.pc += count;
        }

        public bool CheckStackOverflow() {
            if (stackTop >= stack.Length) {
                ReportError("Stack Overflow.");
                return false;
            }
            return true;
        }

        public bool CheckStackEmpty() {
            if (stackTop <= 0) {
                ReportError("Stack Empty.");
                return false;
            }
            return true;
        }

        public void ReportError(string msg) {
            success = false;
            this.message = msg;
        }

        public void Push(Value value) {
            if (CheckStackOverflow()) {
                stack[stackTop] = value;
                stackTop += 1;
            }            
        }

        public Value Pop() {
            if (CheckStackEmpty()) {
                stackTop -= 1;
                return stack[stackTop];
            }
            return null;          
        }

    }
}
