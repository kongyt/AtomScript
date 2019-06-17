namespace AtomScript.VM {

    public class ValueTable {
        public int count;
        public int capacity;
        public Value[] data;

        public void Reset() {
            count = 0;
            capacity = 0;
            data = null;
        }
    }

}
