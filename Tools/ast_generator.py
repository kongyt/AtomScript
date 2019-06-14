#!/usr/bin/python
#encoding: utf-8

import sys
import os
from base_generator import *

baseNS = "AtomScript"

def gen_all_expr():
    out_dir = "../AtomScript/AST/Expression/"
    ns = baseNS + ".AST.Expression"

    useScanner = baseNS + ".Scanner"

    # 基类
    c = CommonClass(ns, "Expr", "IVisitable")
    c.setAbs()
    c.addFunc("public virtual", "void", "Accept", [("AstVisitor", "visitor")], [])
    c.writeTo(out_dir + c.className + ".cs")

    # 一元表达式
    c = CommonClass(ns, "UnaryExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 二元表达式
    c = CommonClass(ns, "BinaryExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Expr", "left")
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 字面值
    c = CommonClass(ns, "LiteralExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Token", "literal")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 括号
    c = CommonClass(ns, "GroupingExpr", "Expr")
    c.addProp("Expr", "expression")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 逻辑
    c = CommonClass(ns, "LogicalExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Expr", "left")
    c.addProp("Token", "op")
    c.addProp("Expr", "right")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 变量
    c = CommonClass(ns, "VariableExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Token", "name")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    # 赋值
    c = CommonClass(ns, "AssignExpr", "Expr")
    c.addUsing(useScanner)
    c.addProp("Token", "name")
    c.addProp("Expr", "value")
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

    c = CommonClass(ns, "ExpressionStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "expr")
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

    c = CommonClass(ns, "WhileStmt", "Stmt")
    c.addUsing(usingExpr)
    c.addProp("Expr", "condition")
    c.addProp("Stmt", "body")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "VarDeclarationStmt", "Stmt")
    c.addUsing(usingScanner)
    c.addUsing(usingExpr)
    c.addProp("Token", "name")
    c.addProp("Expr", "initializer")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

    c = CommonClass(ns, "BlockStmt", "Stmt")
    c.addUsing(usingGeneric)
    c.addUsing(usingExpr)
    c.addProp("List<Stmt>", "stmts")
    c.addFunc("public override", "void", "Accept", [("AstVisitor", "visitor")], ["visitor.Visit(this);"])
    c.writeTo(out_dir + c.className + ".cs")

def gen_ast():
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

    usingStmt = baseNS + ".AST.Statement"

    c = CommonClass(ns, "Ast", "IVisitable")
    c.addUsing(usingStmt)
    c.addProp("Stmt", "root")
    c.addFunc("public", "void", "Accept", [("AstVisitor", "visitor")], ["root.Accept(visitor);"])
    c.writeTo(out_dir + c.className + ".cs")

def gen_ast_visitor():
    out_dir = "../AtomScript/AST/"
    ns = baseNS + ".AST"

    c = CommonClass(ns, "AstVisitor")
    c.setAbs()
    c.addUsing(baseNS + ".AST.Expression")
    c.addUsing(baseNS + ".AST.Statement")

    c.addFunc("public virtual", "void", "Visit", [("UnaryExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("BinaryExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("LiteralExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("GroupingExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("LogicalExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("VariableExpr", "expr")], [])
    c.addFunc("public virtual", "void", "Visit", [("AssignExpr", "expr")], [])

    c.addFunc("public virtual", "void", "Visit", [("ExpressionStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("IfStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("PrintStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("WhileStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("VarDeclarationStmt", "stmt")], [])
    c.addFunc("public virtual", "void", "Visit", [("BlockStmt", "stmt")], [])
    c.writeTo(out_dir + c.className + ".cs")




if __name__ == "__main__":
    gen_all_expr()
    gen_all_stmt()
    gen_ast_visitor()
    gen_ast()

