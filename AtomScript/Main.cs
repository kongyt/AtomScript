using System;
using System.IO;
using System.Collections.Generic;

using AtomScript.AST;
using AtomScript.Scanner;
using AtomScript.Parser;
using AtomScript.Runtime;

namespace AtomScript
{
    class Program
    {
        private Scanner.Scanner scanner;
        private Parser.Parser parser;
        private Compiler.Compiler compiler;
        private VM vm;
        static void Main(string[] args)
        {
            if (args.Length > 2) {
                Console.WriteLine("Usage: atom [source]");
                System.Environment.Exit(1);
            }

            Program program = new Program();
            program.Start();
            if (args.Length == 1) {
                program.Run();
            } else if (args.Length == 2) {
                program.RunFile(args[1]);
            } else {
                program.RunFile("main.atom");
            }
            program.Close();
        }

        public void Start() {
            Console.WriteLine("Program Start.");

            scanner = new Scanner.Scanner();
            parser = new Parser.Parser();
            compiler = new Compiler.Compiler();

            vm = new VM();
            vm.Init();
        }

        public void Close() {
            vm.Destroy();
            Console.WriteLine("Program Close.");
        }

        public void Run() {
            while (true) {
                Console.Write(">");
                string code = Console.ReadLine();
                if (code == "quit") {
                    Console.WriteLine("bye.");
                    break;
                }
                Interpret(code);
            }
        }

        public void RunFile(string file) {
            string code = File.ReadAllText(file);
            InterpretResult result = Interpret(code);
        }

        public InterpretResult Interpret(string source) {
            // 扫描
            ScanResult scanRes = scanner.ScanTokens(source);
            if (scanRes.success == false) {
                List<LexicalError> errors = scanRes.errors;
                for (int i = 0; i < errors.Count; i++) {
                    Report("LexicalError:", errors[i].line, errors[i].column, errors[i].message);
                }
                return InterpretResult.LEXICAL_ERROR;
            }

            // 解析
            ParseResult parseRes = parser.Parse(scanRes.tokens);
            if (parseRes.success == false) {
                List<SyntaxError> errors = parseRes.errors;
                for (int i = 0; i < errors.Count; i++) {
                    Report("SyntaxError:", errors[i].line, errors[i].column, errors[i].message);
                }
                return InterpretResult.SYNTAX_ERROR;
            }

            new AstPrinter().Print(parseRes.ast);

            // 编译
            CompileResult compileResult = compiler.Compile(parseRes.ast);
            if (compileResult.success == false) {
                Report("CompileError:", compileResult.message);
                return InterpretResult.COMPILE_ERROR;
            }

            // 执行
            ExecuteResult executeResult = vm.Execute(compileResult.chunk);
            if (executeResult.success == false) {
                Report("RuntimeError:", executeResult.message);
                return InterpretResult.RUNTIME_ERROR;
            }

            Console.Write("Press Any Key To Continue.");
            Console.Read();
            return InterpretResult.SUCCESS;
        }

        public void Report(string prefix, int line, int column, string message) {
            Report(prefix, "[position " + line + ":" + column + "] " +message);
        }

        public void Report(string prefix, string message) {
            Console.WriteLine(prefix + " " + message);
        }
    }
}
