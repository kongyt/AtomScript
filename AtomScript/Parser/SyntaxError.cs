namespace AtomScript.Parser {

    class SyntaxError {
        public int line;
        public int column;
        public string message;

        public SyntaxError(int line, int column, string message) {
            this.line = line;
            this.column = column;
            this.message = message;
        }

    }

}