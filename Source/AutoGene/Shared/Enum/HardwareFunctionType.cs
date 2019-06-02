namespace Shared.Enum
{
    public enum HardwareFunctionType
    {
        CycleStep = 0,
        StrikeOn = 1,
        StrikeOff = 2,
        HoldOn = 3,
        HoldOff = 4,
        Valve = 5,
        ActivateChannel = 6,
        CloseAllValves = 7,
        AAndTetToCol = 8,
        TAndTetToCol = 9,
        CAndTetToCol = 10,
        GAndTetToCol = 11,
        BAndTetToCol = 12,

        SyringePumpInit = 13,
        SyringePumpFin = 14,
        SyringePumpDraw = 15,

        TrayOut = 16,
        TrayIn = 17,
        TrayLightOn = 18,
        TrayLightOff = 19,

        GSValve = 20,
    }
}