using System;
using System.IO;
using System.Collections.Generic;

namespace AtomScript
{
    class AtomVM
    {
        static void Main(string[] args)
        {
            if (args.Length > 2) {
                Console.WriteLine("Usage: atom [source]");
                Environment.Exit(1);
            }

            AtomVM vm = new AtomVM();
            vm.Start();
            if (args.Length == 1) {
                vm.Run();
            } else if (args.Length == 2) {
                vm.RunFile(args[1]);
            } else {
                vm.RunFile("main.atom");
            }
            vm.Close();
        }

        public void Start() {
            Console.WriteLine("AtomVM Start.");
        }

        public void Close() {
            Console.WriteLine("AtomVM Close.");
        }

        public void Run() {
            while (true) {
                Console.Write(">");
                string code = Console.ReadLine();
                if (code == "quit") {
                    Console.WriteLine("bye.");
                    break;
                }
            }
        }

        public void RunFile(string file) {
            string code = File.ReadAllText(file);
            InterpretResult result = Interpret(code);
        }

        public InterpretResult Interpret(string source) {
            CompileResult compileResult = Compile(source);
            if (compileResult.success == false) {
                return InterpretResult.COMPILE_ERROR;
            }

            ExecuteResult executeResult = Execute(compileResult.byteCode);
            if (executeResult.success == false) {
                return InterpretResult.RUNTIME_ERROR;
            }

            return InterpretResult.SUCCESS;
        }

        private CompileResult Compile(string source) {
            CompileResult result = new CompileResult();

            Scanner scanner = new Scanner();
            ScanResult scanRes = scanner.ScanTokens(source);
            List<Token> tokens = scanRes.tokens;
            for (int i = 0; i < tokens.Count; i++) {
                Console.WriteLine(tokens[i]);
            }

            if (scanRes.success) {
                result.success = true;
            } else {
                result.success = false;
                List<ScanError> errors = scanRes.errors;
                for (int i = 0; i < errors.Count; i++) {
                    Error(errors[i].line, errors[i].errorStr);
                }
            }

            return result;
        }

        private ExecuteResult Execute(ByteCode byteCode) {
            ExecuteResult result = new ExecuteResult();
            result.success = true;
            return result;
        }

        public void Error(int line, string message) {
            Report(line, "", message);
        }

        public string Report(int line, string where, string message) {
            return "[line ]" + line + "] Error" + where + ": " + message;
        }
    }
}
