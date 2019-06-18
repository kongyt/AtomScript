using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
     class Class : Obj{
        public string className;
        public Table<Function> functionTable = new Table<Function>();           // 方法表，最多65535个

        public bool AddMethod(Function method, out ushort index) {
            if (functionTable.count >= ushort.MaxValue) {
                index = 0;
                return false;
            }
            index = (ushort)functionTable.Add(method);
            return true;
        }

        public Function GetMethod(ushort index) {
            if (index < 0 || index >= functionTable.count) {
                
            }
            return functionTable.data[index];
        }
    }
}
