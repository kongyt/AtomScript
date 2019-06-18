using System;
using System.IO;
using System.Collections.Generic;

using AtomScript.AST;
using AtomScript.Scanner;
using AtomScript.Parser;
using AtomScript.Compiler;
using AtomScript.Runtime;

namespace AtomScript
{
    class Program
    {
        private Scanner.Scanner scanner;
        private Parser.Parser parser;
        private Compiler.Compiler compiler;
        private VM vm;

        private Ast lastAst;

        static void Main(string[] args)
        {
            if (args.Length > 2) {
                Console.WriteLine("Usage: atom [source]");
                System.Environment.Exit(1);
            }

            Program program = new Program();
            program.Start();
            program.Run();
            //if (args.Length == 1) {
            //    program.Run();
            //} else if (args.Length == 2) {
            //    program.RunFile(args[1]);
            //} else {
            //    program.RunFile("main.atom");
            //}
            program.Close();
        }

        public void Start() {
            Console.WriteLine("Atom Script.");

            scanner = new Scanner.Scanner();
            parser = new Parser.Parser();
            compiler = new Compiler.Compiler();

            vm = new VM();
            vm.Init();
        }

        public void Close() {
            vm.Destroy();
        }

        public void Run() {
            while (true) {
                Console.Write(">");
                string code = Console.ReadLine();
                if (code == "quit") {
                    Console.WriteLine("bye.");
                    break;
                } else if (code == "debug") {
                    vm.Debug();
                } else if (code == "ast") {
                    if (lastAst != null) {
                        new AstPrinter().Print(lastAst);
                    } else {
                        Console.WriteLine("no ast code to print.");
                    }                    
                } else {
                    Interpret(code);
                }                
            }
        }

        public void RunFile(string file) {
            string code = File.ReadAllText(file);
            InterpretResult result = Interpret(code);
            Console.Write("Press Any Key To Continue.");
            Console.Read();
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

            lastAst = parseRes.ast;
            // 编译
            CompileResult compileRes = compiler.Compile(parseRes.ast);
            if (compileRes.success == false) {
                List<CompileError> errors = compileRes.errors;
                for (int i = 0; i < errors.Count; i++) {
                    Report("CompileError:", errors[i].line, errors[i].column, errors[i].message);
                }
                return InterpretResult.COMPILE_ERROR;
            }

            // 执行
            ExecuteResult executeResult = vm.Execute(compileRes.chunk);
            if (executeResult.success == false) {
                Report("RuntimeError:", executeResult.message);
                return InterpretResult.RUNTIME_ERROR;
            }
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
