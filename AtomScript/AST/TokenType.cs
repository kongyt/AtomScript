namespace AtomScript.AST {

    enum TokenType {
        UNDEFINED,                               // 未定义

        // 单字符Token
        LEFT_PAREN,                              // 左括号
        RIGHT_PAREN,                             // 右括号
        LEFT_BRACE,                              // 左大括号
        RIGHT_BRACE,                             // 右大括号
        COMMA,                                   // 逗号
        DOT,                                     // 点
        MINUS,                                   // 减
        PLUS,                                    // 加
        SEMICOLON,                               // 分号
        SLASH,                                   // 斜线
        STAR,                                    // 星号
        COLON,                                   // 冒号

        // 一个或两个字符Token
        BANG,                                    // 相反的
        BANG_EQUAL,                              // 不等于
        EQUAL,                                   // 赋值
        EQUAL_EQUAL,                             // 等于
        GREATER,                                 // 大于
        GREATER_EQUAL,                           // 大于等于
        LESS,                                    // 小于
        LESS_EQUAL,                              // 小于等于

        // 值类型
        IDENTIFIER,                              // 标识符
        STRING,
        NUMBER,

        // 关键字
        AND,
        CLASS,
        ELSE,
        FALSE,
        FUNC,
        FOR,
        IF,
        NIL,
        OR,
        PRINT,
        RETURN,
        SUPER,
        THIS,
        TRUE,
        VAR,
        WHILE,
        EOF,
    }

}