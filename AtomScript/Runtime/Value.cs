namespace AtomScript.Runtime {

    struct Value {
        public ValueType type;
        public object data;

        public Value(bool value) {
            type = ValueType.BOOL;
            data = value;
        }

        public Value(double value) {
            type = ValueType.NUMBER;
            data = value;
        }

        public Value(string value) {
            type = ValueType.STRING;
            data = value;
        }

        public Value(Obj obj) {
            type = ValueType.OBJECT;
            data = obj;
        }

        public bool AsBool() {
            return (bool)data;
        }

        public double AsNumber() {
            return (double)data;
        }

        public string AsString() {
            return (string)data;
        }

        public Obj AsObj() {
            return (Obj)data;
        }

        public bool IsBool() {
            return type == ValueType.BOOL;
        }

        public bool IsNil() {
            return type == ValueType.NIL;
        }

        public bool IsNumber() {
            return type == ValueType.NUMBER;
        }

        public bool IsString() {
            return type == ValueType.STRING;
        }

        public bool IsObj() {
            return type == ValueType.OBJECT;
        }

        public bool IsFalsey() {
            return IsNil() || (IsBool() && AsBool() == false);
        }
    }
}
