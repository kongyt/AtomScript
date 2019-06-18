using System;
using System.Collections.Generic;
using AtomScript.AST;

namespace AtomScript.Scanner {

    class Scanner {

        private string source;
        private bool success;
        private List<Token> tokens;
        private List<LexicalError> errors;
        private int start;
        private int current;
        private int line;
        private int column;


        private void Reset() {
            source = null;
            success = true;
            tokens = new List<Token>();
            errors = new List<LexicalError>();
            start = 0;
            current = 0;
            line = 1;
            column = 0;
        }

        public ScanResult ScanTokens(string source) {
            Reset();
            this.source = source;
            while (IsAtEnd() == false) {
                start = current;
                ScanToken();
            }
            MakeToken(TokenType.EOF);
            return new ScanResult(success, tokens, errors);
        }

        private void ScanToken() {
            Advance();
            char c = ReadChar();
            switch (c) {
                case '(': MakeToken(TokenType.LEFT_PAREN); break;
                case ')': MakeToken(TokenType.RIGHT_PAREN); break;
                case '{': MakeToken(TokenType.LEFT_BRACE); break;
                case '}': MakeToken(TokenType.RIGHT_BRACE); break;
                case ',': MakeToken(TokenType.COMMA); break;
                case '.': MakeToken(TokenType.DOT); break;
                case '-': MakeToken(TokenType.MINUS); break;
                case '+': MakeToken(TokenType.PLUS); break;
                case ';': MakeToken(TokenType.SEMICOLON); break;
                case '*': MakeToken(TokenType.STAR); break;
                case ':': MakeToken(TokenType.COLON); break;

                case '!': MakeToken(MatchChar('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': MakeToken(MatchChar('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': MakeToken(MatchChar('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': MakeToken(MatchChar('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;

                case '/':
                    if (MatchChar('/')) {
                        JumpToLineEnd();
                    } else {
                        MakeToken(TokenType.SLASH);
                    }
                    break;
                case '"':
                    if (MatchString()) {
                        MakeToken(TokenType.STRING);
                    } else {
                        ReportError(line, column, "Unterminated string.");
                    }
                    break;
                default:
                    if (IsNewLine(c)) { // 新行
                        line += 1;
                        column = 0;
                    } else if (IsWhiteSpace(c)) {
                        // 空白符
                    } else if (IsDigit(c)) {    // 数值
                        if (MatchNumeric()) {
                            MakeToken(TokenType.NUMBER);
                        } else {
                            ReportError(line, column, "Unexpected character. (" + c + ")");
                        }
                    } else if (IsAlpha(c)) {
                        if (MatchAndMakeKeywords(c)) {
                            break;
                        } else if (MatchWord()) {
                            MakeToken(TokenType.IDENTIFIER);
                        } else {
                            ReportError(line, column, "Unexpected character. (" + c + ")");
                        }                       
                    } else {
                        ReportError(line, column, "Unexpected character. (" + c + ")");
                    }
                    break;
            }
        }

        private bool IsAtEnd() {
            return current >= source.Length;
        }

        private void Advance() {
            current++;
            column++;
        }

        private void SkipWhiteSpace() {
            while (IsAtEnd() == false && IsWhiteSpace(ReadChar())) {
                Advance();
            }
        }

        private void JumpToLineEnd() {
            while (IsAtEnd() == false && IsNewLine(ReadChar()) == false) {
                Advance();
            }
        }

        private char ReadChar() {
            return source[current - 1];
        }

        private char ReadNextChar() {
            if (IsAtEnd()) {
                return '\0';
            } else {
                return source[current];
            }
        }

        private bool IsNewLine(char c) {
            return c == '\n';
        }

        private bool IsWhiteSpace(char c) {
            return c == '\r' || c == ' ' || c == '\t';
        }

        private bool IsAlpha(char c) {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private bool IsDigit(char c) {
            return c >= '0' && c <= '9';
        }

        private bool IsAlphaNumeric(char c) {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool MatchChar(char expected) {
            if (ReadNextChar() == expected) {
                Advance();
                return true;
            } else {
                return false;
            }
        }

        private bool MatchAndMakeKeywords(char c) {
            switch (c) {
                case 'a': return MatchKeyword("nd", TokenType.AND);
                case 'c': return MatchKeyword("lass", TokenType.CLASS);
                case 'e': return MatchKeyword("lse", TokenType.ELSE);
                case 'f': return MatchKeyword("alse", TokenType.FALSE) || MatchKeyword("or", TokenType.FOR) || MatchKeyword("unc", TokenType.FUNC);
                case 'i': return MatchKeyword("f", TokenType.IF);
                case 'n': return MatchKeyword("il", TokenType.NIL);
                case 'o': return MatchKeyword("r", TokenType.OR);
                case 'p': return MatchKeyword("rint", TokenType.PRINT);
                case 'r': return MatchKeyword("eturn", TokenType.RETURN);
                case 's': return MatchKeyword("uper", TokenType.SUPER);
                case 't': return MatchKeyword("his", TokenType.THIS) || MatchKeyword("rue", TokenType.TRUE); 
                case 'v': return MatchKeyword("ar", TokenType.VAR);
                case 'w': return MatchKeyword("hile", TokenType.WHILE);
            }
            return false;
        }

        private bool MatchKeyword(string rest, TokenType type) {
            int length = rest.Length;
            if (this.current + length >= this.source.Length) {
                return false;
            }

            for (int i = 0; i < length; i++) {
                if (source[current + i] != rest[i]) {
                    return false;
                }
            }
            current += length;
            column += length;
            MakeToken(type);
            return true;
        }

        private bool MatchString() {
            bool match = false;
            while (IsAtEnd() == false) {
                Advance();
                char c = ReadChar();
                if (IsNewLine(c)) {
                    break;
                }

                if (c == '"') {
                    // 字符串后面不能接字母数字
                    if (IsAlphaNumeric(ReadNextChar()) == false) {
                        match = true;                        
                    }
                    break;
                }
            }
            return match;
        }

        private bool MatchNumeric() {
            bool match = true;
            bool hasDot = false;
            while (IsAtEnd() == false) {
                char c = ReadNextChar();
                if (IsDigit(c)) {
                    Advance();
                } else {
                    if (c == '.') {
                        Advance();
                        hasDot = true;
                    }
                    break;
                }
            }

            if (hasDot) {
                // 如果有小数点，下一个字符必须是数字
                if (IsDigit(ReadNextChar())) {
                    do {
                        Advance();
                    } while (IsDigit(ReadNextChar()));
                } else {
                    match = false;
                }
            }
            if (match == true) {
                // 数值后面不能接字母或引号
                char c = ReadNextChar();
                if (IsAlpha(c) || c == '"') {
                    match = false;
                }
            }

            return match;
        }

        private bool MatchWord() {
            bool match = true;
            while (IsAtEnd() == false) {
                if (IsAlphaNumeric(ReadNextChar()) == false) {
                    break;
                }

                Advance();
            }

            char c = ReadNextChar();
            if (c == '"') {
                match = false;
            }
            return match;
        }

        private string ReadText() {
            return source.Substring(start, current - start);
        }

        private object GetLiteral(TokenType type, string text) {
            object literal = null;
            switch (type) {
                case TokenType.STRING:
                    literal = text.Substring(1, text.Length - 2);
                    break;
                case TokenType.NUMBER:
                    literal = Convert.ToDouble(text);
                    break;
            }
            return literal;
        }

        private void MakeToken(TokenType type) {
            if (type == TokenType.EOF) {
                tokens.Add(new Token(type, "", null, line, column));
            } else {
                string text = ReadText();
                object literal = GetLiteral(type, text);
                tokens.Add(new Token(type, text, literal, line, column));
            }
        }

        private void ReportError(int line, int column, string message) {
            success = false;
            errors.Add(new LexicalError(line, column, message));
        }
    }
}
