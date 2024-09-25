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
    public required Racer Racer { get; init; }

    [ObservableProperty]
    private int _lap;

    [ObservableProperty]
    private int _position;

    [ObservableProperty]
    private PositionChange _positionChange;
}