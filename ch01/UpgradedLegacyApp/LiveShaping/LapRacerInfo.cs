using CommunityToolkit.Mvvm.ComponentModel;

namespace LiveShaping;

public enum PositionChange
{
    None,
    Up,
    Down,
    Out
}

public partial class LapRacerInfo : ObservableObject
{
    public required Racer Racer { get; set; }

    [ObservableProperty]
    public partial int Lap {  get; set; }

    [ObservableProperty]
    public partial int Position { get; set; }

    [ObservableProperty]
    public partial PositionChange PositionChange { get; set; }
}