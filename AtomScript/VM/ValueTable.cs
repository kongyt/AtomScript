using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript {

    public class ValueTable<T> {
        public int count;
        public int capacity;
        public T[] data;

        public void Reset() {
            count = 0;
            capacity = 0;
            data = null;
        }
    }

}
