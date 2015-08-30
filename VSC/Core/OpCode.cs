namespace Vsc.Core
{
    public enum OpCode
    {
        STOP,

        LDC_X,
        LDC_Y,
        LDC_P,

        LDA_X,
        LDA_Y,
        LDA_P,

        LDP_X,
        LDP_Y,

        STA_X,
        STA_Y,
        STA_Z,
        STA_P,

        STP_X,
        STP_Y,
        STP_Z,

        IN_X,
        IN_Y,

        OUT_X,
        OUT_Y,
        OUT_Z,

        CZP,
        CPX,
        CPY,

        JMP,
        JIZ,
        JIP,
        JIN,

        ADD,
        SUB,
        MUL,
        DIV
    }
}
