#!/usr/bin/python
#encoding: utf-8

import sys
import os
from base_generator import *

baseDir = "AtomScript"
baseNS = "AtomScript"

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
    gen_lexical_error()
    gen_scan_result()
