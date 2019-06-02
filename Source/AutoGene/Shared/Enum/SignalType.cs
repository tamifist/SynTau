namespace Shared.Enum
{
    public enum SignalType
    {
        ActivateValves = 0,
        DeactivateAllValves,

        StartOligoSynthesisProcess,
        SuspendOligoSynthesisProcess,
        StopOligoSynthesisProcess,

        StartGeneSynthesisProcess,
        SuspendGeneSynthesisProcess,
        StopGeneSynthesisProcess,

        SyringePump,

        TrayOut,
        TrayIn,
        TrayLightOn,
        TrayLightOff,

        GSValve,
    }
}