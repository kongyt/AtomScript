namespace AtomScript.Scanner {

    class LexicalError {
        public int line;
        public int column;
        public string message;

        public LexicalError(int line, int column, string message) {
            this.line = line;
            this.column = column;
            this.message = message;
        }

    }

}