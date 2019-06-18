using System;
using System.Collections.Generic;
using System.Text;

namespace AtomScript.Runtime {
    enum OpCode {
        RETURN,         // 返回
        CONSTANT,       // 加载常量到Stack
        NIL,            // 加载NIL到Stack
        TRUE,           // 加载True到Stack
        FALSE,          // 加载False到Stack
        DEF_GLOBAL,     // 定义全局变量
        GET_GLOBAL,     // 获取全局变量
        SET_GLOBAL,     // 赋值全局变量
        NEGATE,         // 负
        ADD,            // +
        SUB,            // -
        MUL,            // *
        DIV,            // /
        PRINT,          // 打印
        EQUAL,          // ==
        NOT_EQUAL,      // !=
        GREATER,        // >
        GREATER_EQUAL,  // >=
        LESS,           // <
        LESS_EQUAL,     // <=
    }
}
