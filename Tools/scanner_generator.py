#!/usr/bin/python
#encoding: utf-8

import sys
import os
from base_generator import *

baseDir = "AtomScript"
baseNS = "AtomScript"

def gen_token_type():
    out_dir = "../" + baseDir + "/Scanner/"
    ns = baseNS + ".Scanner"

    c = CommonEnum(ns, "TokenType")
    c.addEnum("UNDEFINED", "未定义")

    c.addComment("单字符Token")
    c.addEnum("LEFT_PAREN", "左括号")
    c.addEnum("RIGHT_PAREN", "右括号")
    c.addEnum("LEFT_BRACE", "左大括号")
    c.addEnum("RIGHT_BRACE", "右大括号")
    c.addEnum("COMMA", "逗号")
    c.addEnum("DOT", "点")
    c.addEnum("MINUS", "减")
    c.addEnum("PLUS", "加")
    c.addEnum("SEMICOLON", "分号")
    c.addEnum("SLASH", "斜线")
    c.addEnum("STAR", "星号")

    c.addComment("一个或两个字符Token")
    c.addEnum("BANG", "相反的")
    c.addEnum("BANG_EQUAL", "不等于")
    c.addEnum("EQUAL", "赋值")
    c.addEnum("EQUAL_EQUAL", "等于")
    c.addEnum("GREATER", "大于")
    c.addEnum("GREATER_EQUAL", "大于等于")
    c.addEnum("LESS", "小于")
    c.addEnum("LESS_EQUAL", "小于等于")

    c.addComment("值类型")
    c.addEnum("IDENTIFIER", "标识符")
    c.addEnum("STRING")
    c.addEnum("NUMBER")

    c.addComment("关键字")
    c.addEnum("AND")
    c.addEnum("CLASS")
    c.addEnum("ELSE")
    c.addEnum("FALSE")
    c.addEnum("FUNC")
    c.addEnum("FOR")
    c.addEnum("IF")
    c.addEnum("NIL")
    c.addEnum("OR")
    c.addEnum("PRINT")
    c.addEnum("RETURN")
    c.addEnum("SUPER")
    c.addEnum("THIS")
    c.addEnum("TRUE")
    c.addEnum("VAR")
    c.addEnum("WHILE")
    c.addEnum("EOF")

    c.writeTo(out_dir + c.enumName+ ".cs")

def gen_token():
    out_dir = "../" + baseDir + "/Scanner/"
    ns = baseNS + ".Scanner"

    c = CommonClass(ns, "Token", None)
    c.addProp("TokenType", "type")
    c.addProp("string", "lexeme")
    c.addProp("object", "literal")
    c.addProp("int", "line")
    c.addProp("int", "column")
    c.writeTo(out_dir + c.className + ".cs")

def gen_lexical_error():
    out_dir = "../" + baseDir + "/Scanner/"
    ns = baseNS + ".Scanner"

    c = CommonClass(ns, "LexicalError", None)
    c.addProp("int", "line")
    c.addProp("int", "column")
    c.addProp("string", "message")
    c.writeTo(out_dir + c.className + ".cs")

def gen_scan_result():
    out_dir = "../" + baseDir + "/Scanner/"
    ns = baseNS + ".Scanner"

    c = CommonClass(ns, "ScanResult", None)
    c.addUsing("System.Collections.Generic")
    c.addProp("bool", "success")
    c.addProp("List<Token>", "tokens")
    c.addProp("List<LexicalError>", "errors")
    c.writeTo(out_dir + c.className + ".cs")

if __name__ == "__main__":
    gen_token_type()
    gen_token()
    gen_lexical_error()
    gen_scan_result()
