#!/usr/bin/python
#encoding: utf-8

import sys
import os

class CommonEnum:
    def __init__(self, namespace, enumName):
        self.namespace = namespace
        self.enumName = enumName
        self.enumList = []

    def whitespace(self, num):
        rs = ""
        for i in range(num):
            rs += ' '
        return rs

    def addEnum(self, enumType, enumComment = None):
        self.enumList.append((enumType, enumComment))
        return self

    def addComment(self, comment):
        self.enumList.append((None, comment))
        return self

    def toString(self):
        # namespace
        code = "namespace " + self.namespace + " {\n\n"
        # enum
        code = code + self.whitespace(4) + "enum " + self.enumName + " {\n"
        # 枚举值或单行注释
        for item in self.enumList:
            if item[0] is None:
                code = code + "\n" + self.whitespace(8) + "// " + item[1] + "\n"
            elif item[1] is None:
                code = code + self.whitespace(8) + item[0] + ",\n"
            else:
                code = code + self.whitespace(8) + item[0] + ","
                spNum = 40 - len(item[0])
                for i in range(spNum):
                    code += " "
                code = code + "// " + item[1] + "\n"
        code = code + self.whitespace(4) + "}\n\n"
        code = code + "}"
        return code

    def writeTo(self, fileName):
        print "============== generate enum ============="
        code = self.toString()
        print code
        print "============ generate completed ==========="
        f = open(fileName, "w")
        f.write(self.toString())
        f.close()
        print "============= save completed =============="


class CommonClass:
    def __init__(self, namespace, className, baseClassName = None):
        self.namespace = namespace
        self.className = className
        self.baseClassName = baseClassName
        self.isAbs = False
        self.usingList = []
        self.propList = []
        self.privatePropList = []
        self.funcs = []

    def whitespace(self, num):
        rs = ""
        for i in range(num):
            rs += ' '
        return rs

    def setAbs(self):
        self.isAbs = True
        return self

    def addUsing(self, using):
        self.usingList.append(using)

    def addProp(self, propType, propName):
        self.propList.append((propType, propName))
        return self

    def addPrivateProp(self, propType, propName):
        self.privatePropList.append((propType, propName))
        return self

    def addFunc(self, accessModifier, returnType, name, argList = [], codeLines = [], comments = None):
        self.funcs.append((accessModifier, returnType, name, argList, codeLines, comments))
        return self

    def toString(self):
        # using
        code = ""
        if len(self.usingList) > 0:
            for item in self.usingList:
                code = code + "using " + item + ";\n"
            code += "\n"
        # namespace
        code = code + "namespace " + self.namespace + " {\n\n"
        # class
        code = code + self.whitespace(4)
        if self.isAbs:
            code = code + "abstract "
        code = code + "class " + self.className
        if self.baseClassName is not None:
            code = code + " : " + self.baseClassName
        code = code + " {\n"

        # 成员变量
        for item in self.propList:
            code = code + self.whitespace(8) + "public " + item[0] + " " + item[1] + ";\n"

        if len(self.privatePropList) > 0:
            code = code + "\n"
            for item in self.privatePropList:
                code = code + self.whitespace(8) + "private " + item[0] + " " + item[1] + ";\n"
        
        code = code + "\n"
        # 构造函数
        if len(self.propList) > 0:
            code = code + self.whitespace(8) + "public " + self.className + "("
            isFirst = True
            for item in self.propList:
                if isFirst == False:
                    code = code + ", "
                else:
                    isFirst = False
                code = code + item[0] + " " + item[1]
            code = code + ") {\n"
            for item in self.propList:
                code = code + self.whitespace(12) + "this." + item[1] + " = " + item[1] + ";\n"
            code = code + self.whitespace(8) + "}\n\n"

        firstFlag = True
        # 普通函数
        for item in self.funcs:
            if firstFlag:
                firstFlag = False
            else:
                code = code + "\n"
            if item[5] is not None:
                code = code + self.whitespace(8) + "//" + item[5]
            code = code + self.whitespace(8) + item[0] + " " + item[1] + " " + item[2] + "("
            if len(item[3]) > 0:
                isFirst = True
                for arg in item[3]:
                    if isFirst:
                        isFirst = False
                    else:
                        code += ", "
                    code = code + arg[0] + " " + arg[1]
            code = code + ") {\n"

            if len(item[4]) > 0:
                for line in item[4]:
                    code = code + self.whitespace(12) + line + "\n"
            else:
                code += "\n"
            code = code + self.whitespace(8) + "}\n"

        code = code + self.whitespace(4) + "}\n\n"
        code = code + "}"
        return code

    def writeTo(self, fileName):
        print "============== generate class ============="
        code = self.toString()
        print code
        print "============ generate completed ==========="
        f = open(fileName, "w")
        f.write(self.toString())
        f.close()
        print "============= save completed =============="