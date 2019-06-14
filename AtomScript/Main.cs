using System;
using System.IO;
using System.Collections.Generic;

using AtomScript.AST;
using AtomScript.Scanner;
using AtomScript.Parser;

namespace AtomScript
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 2) {
                Console.WriteLine("Usage: atom [source]");
                Environment.Exit(1);
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
        }

        public void Close() {
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

            Scanner.Scanner scanner = new Scanner.Scanner();
            ScanResult scanRes = scanner.ScanTokens(source);
            Console.WriteLine("=========== Tokens ==========");
            List<Token> tokens = scanRes.tokens;
            for (int i = 0; i < tokens.Count; i++) {
                Console.WriteLine("" + tokens[i].line + " " + tokens[i].column + " " + tokens[i].type + " " + tokens[i].lexeme);
            }

            if (scanRes.success) {
                Parser.Parser parser = new Parser.Parser();
                ParseResult parseRes = parser.Parse(scanRes.tokens);
                if (parseRes.success) {
                    Console.WriteLine("=========== Code ==========");
                    Ast ast= parseRes.ast;
                    new AstPrinter().Print(ast);
                    Console.WriteLine("============ End ==========");

                    result.success = true;
                } else {
                    result.success = false;
                    List<SyntaxError> errors = parseRes.errors;
                    for (int i = 0; i < errors.Count; i++) {
                        Report(errors[i].line, errors[i].column, errors[i].message);
                    }
                }
            } else {
                result.success = false;
                List<LexicalError> errors = scanRes.errors;
                for (int i = 0; i < errors.Count; i++) {
                    Report(errors[i].line, errors[i].column, errors[i].message);
                }
            }

            return result;
        }

        private ExecuteResult Execute(ByteCode byteCode) {
            ExecuteResult result = new ExecuteResult();
            result.success = true;
            return result;
        }

        public void Report(int line, int column, string message) {
            Console.WriteLine("[position " + line + ":" + column + "] " +message);
        }
    }
}
