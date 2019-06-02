namespace Data.Contracts.Entities.SystemMonitor
{
    public enum SensorState
    {
        Inactive = 0,
        Active,
        Warning,
        Danger,
        ForceStop
    }
}