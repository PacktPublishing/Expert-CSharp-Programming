﻿namespace LiveShaping;

public enum PositionChange
{
    None,
    Up,
    Down,
    Out
}

public class LapRacerInfo : BindableObject
{
    public required Racer Racer { get; init; }
    private int _lap;
    public int Lap
    {
        get => _lap;
        set => SetProperty(ref _lap, value);
    }
    private int _position;
    public int Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }
    private PositionChange _positionChange;
    public PositionChange PositionChange
    {
        get => _positionChange;
        set => SetProperty(ref _positionChange, value);
    }
}