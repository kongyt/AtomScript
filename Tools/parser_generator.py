#!/usr/bin/python
#encoding: utf-8

import sys
import os
from base_generator import *

baseDir = "AtomScript"
baseNS = "AtomScript"

def gen_syntax_error():
    out_dir = "../" + baseDir + "/Parser/"
    ns = baseNS + ".Parser"

    c = CommonClass(ns, "SyntaxError", None)
    c.addProp("int", "line")
    c.addProp("int", "column")
    c.addProp("string", "message")
    c.writeTo(out_dir + c.className + ".cs")

def gen_parse_result():
    out_dir = "../" + baseDir + "/Parser/"
    ns = baseNS + ".Parser"
    usingExpr = baseNS + ".AST.Expression"
    usingStmt = baseNS + ".AST.Statement"

    c = CommonClass(ns, "ParseResult", None)
    c.addUsing("System.Collections.Generic")
    c.addUsing(usingExpr)
    c.addUsing(usingStmt)
    c.addProp("bool", "success")
    c.addProp("List<Stmt>", "statements")
    c.addProp("List<SyntaxError>", "errors")
    c.writeTo(out_dir + c.className + ".cs")

if __name__ == "__main__":
    gen_syntax_error()
    gen_parse_result()