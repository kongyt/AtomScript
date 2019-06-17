namespace AtomScript
{
    public enum OpCode {
        OP_RETURN,      // Return
        OP_CONSTANT,    // 读取常量
        OP_NIL,         // 读取nil
        OP_TRUE,        // 读取true
        OP_FALSE,       // 读取false
        OP_NOT,         // 取非
        OP_EQUAL,       // 等于
        OP_GREATER,     // 大于
        OP_LESS,        // 小于
        OP_NOT_EQUAL,   // 不等于
        OP_GREATER_EQUAL,// 大于等于
        OP_LESS_EQUAL,  // 小于等于
        OP_ADD,         // 加
        OP_SUB,         // 减
        OP_MUL,         // 乘
        OP_DIV,         // 除
        
    }    
}
