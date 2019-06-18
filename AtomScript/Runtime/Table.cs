using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    class Table<T> {
        public int count;
        public int capacity;
        public T[] data;

        public Table(){
            capacity = 4;
            count = 0;
            data = new T[capacity];
        }

        public Table(int capacity) {
            this.capacity = capacity;
            count = 0;
            data = new T[capacity];
        }

        public int Add(T value) {
            if (count == int.MaxValue) {
                return -1;
            }
            if (capacity < count + 1) {
                GrowCapacity();
            }
            int index = count;
            data[count] = value;
            count += 1;
            return index;
        }

        private void GrowCapacity() {
            int oldCapacity = capacity;
            int newCapacity = capacity < 8 ? 8 : (capacity * 2);
            T[] buf = new T[newCapacity];
            if (oldCapacity > 0) {
                Buffer.BlockCopy(data, 0, buf, 0, oldCapacity);
            }
            data = buf;
            capacity = newCapacity;
        }


    }
}
