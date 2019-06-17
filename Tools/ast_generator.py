#!/usr/bin/python
#encoding: utf-8

import sys
import os
from base_generator import *

baseNS = "AtomScript"

def gen_all_expr():
    out_dir = "../AtomScript/AST/Expression/"
    ns = baseNS + ".AST.Expression"

    usingGeneric = "System.Collections.Generic"

    # 基类
    c = CommonClass(ns, "Expr", "IVisitable")
    c.setAbs()
    c.addFunc("public virtual", "void", "Accept", [("AstVisitor", "visitor")], [])
    c.writeTo(out_dir + c.className + ".cs")

    # 赋值
    c = CommonClass(ns, "AssignExpr", "Expr")
    c.addProp("Token", "name")
    c.addProp("Expr", "value")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 二元表达式
    c = CommonClass(ns, "BinaryExpr", "Expr")
    c.addProp("Expr", "left")
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "CallExpr", "Expr")
    c.addUsing(usingGeneric)
    c.addProp("Expr", "callee")
    c.addProp("Token", "paren")
    c.addProp("List<Expr>", "arguments")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 括号
    c = CommonClass(ns, "GroupingExpr", "Expr")
    c.addProp("Expr", "expression")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # Get
    c = CommonClass(ns, "GetExpr", "Expr")
    c.addProp("Expr", "obj")
    c.addProp("Token", "name")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 字面值
    c = CommonClass(ns, "LiteralExpr", "Expr")
    c.addProp("Token", "literal")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 逻辑
    c = CommonClass(ns, "LogicalExpr", "Expr")
    c.addProp("Expr", "left")
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # Set
    c = CommonClass(ns, "SetExpr", "Expr")
    c.addProp("Expr", "obj")
    c.addProp("Token", "name")
    c.addProp("Expr", "value")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # Super
    c = CommonClass(ns, "SuperExpr", "Expr")
    c.addProp("Token", "keyword")
    c.addProp("Token", "method")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # This
    c = CommonClass(ns, "ThisExpr", "Expr")
    c.addProp("Token", "keyword")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 一元表达式
    c = CommonClass(ns, "UnaryExpr", "Expr")
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 变量
    c = CommonClass(ns, "VariableExpr", "Expr")
    c.addProp("Token", "name")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")



def gen_all_stmt():
    out_dir = "../AtomScript/AST/Statement/"
    ns = baseNS + ".AST.Statement"
    usingExpr = baseNS + ".AST.Expression"
    usingSystem = "System"
    usingGeneric = "System.Collections.Generic"
    usingScanner = baseNS + ".Scanner"

    c = CommonClass(ns, "Stmt", "IVisitable")
    c.setAbs()
    c.addFunc("public virtual", "void", "Accept", [("AstVisitor", "visitor")], [])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "BlockStmt", "Stmt")
    c.addUsing(usingGeneric)
    c.addProp("List<Stmt>", "stmts")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "ClassDeclarationStmt", "Stmt")
    c.addUsing(usingGeneric)
    c.addUsing(usingExpr)
    c.addProp("Token", "name")
    c.addProp("VariableExpr", "superclass")
    c.addProp("List<FuncDeclarationStmt>", "methods")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "ExpressionStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "expr")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "FuncDeclarationStmt", "Stmt")
    c.addUsing(usingGeneric)
    c.addProp("Token", "name")
    c.addProp("List<Token>", "parameters")
    c.addProp("BlockStmt", "body")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "ForStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Stmt", "initializer")
    c.addProp("Expr", "condition")
    c.addProp("Expr", "increment")
    c.addProp("Stmt", "body")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "IfStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "condition")
    c.addProp("Stmt", "thenBranch")
    c.addProp("Stmt", "elseBranch")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "PrintStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "expr")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "ReturnStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Token", "keyword")
    c.addProp("Expr", "value")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "VarDeclarationStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Token", "name")
    c.addProp("Expr", "initializer")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "WhileStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "condition")
    c.addProp("Stmt", "body")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

def gen_ast():
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

    usingGeneric = "System.Collections.Generic"
    usingStmt = baseNS + ".AST.Statement"

    c = CommonClass(ns, "Ast", "IVisitable")
    c.addUsing(usingGeneric)
    c.addUsing(usingStmt)
    c.addProp("List<Stmt>", "stmts")
    c.addFunc("public", "void", "Accept", [("AstVisitor", "visitor")], ["for (int i = 0; i < stmts.Count; i++) {", "    stmts[i].Accept(visitor);", "}"])
    c.writeTo(out_dir + c.className + ".cs")

def gen_ast_visitor():
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

    c = CommonClass(ns, "AstVisitor")
    c.setAbs()
    c.addUsing(baseNS + ".AST.Expression")
    c.addUsing(baseNS + ".AST.Statement")

    c.addFunc("public virtual", "void", "Visit", [("AssignExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("BinaryExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("CallExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("GetExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("GroupingExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("LiteralExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("LogicalExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("SetExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("SuperExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("ThisExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("UnaryExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("VariableExpr", "expr")], [])

    c.addFunc("public virtual", "void", "Visit", [("BlockStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("ClassDeclarationStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("ExpressionStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("ForStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("FuncDeclarationStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("IfStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("PrintStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("ReturnStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("VarDeclarationStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("WhileStmt", "stmt")], [])


    c.writeTo(out_dir + c.className + ".cs")

def gen_token_type():
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

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
    c.addEnum("COLON", "冒号")

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
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

    c = CommonClass(ns, "Token", None)
    c.addProp("TokenType", "type")
    c.addProp("string", "lexeme")
    c.addProp("object", "literal")
    c.addProp("int", "line")
    c.addProp("int", "column")
    c.writeTo(out_dir + c.className + ".cs")


if __name__ == "__main__":
    gen_all_expr()
    gen_all_stmt()
    gen_ast_visitor()
    gen_ast()
    gen_token_type()
    gen_token()

