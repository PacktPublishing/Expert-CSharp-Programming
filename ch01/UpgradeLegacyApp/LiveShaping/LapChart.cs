﻿namespace LiveShaping;

public class LapChart
{
    private readonly Formula1 _f1 = new();
    private List<LapRacerInfo>? _lapInfo;
    private int _currentLap = 0;
    private const int PositionOut = 99;
    private readonly int _maxLaps;
    public LapChart()
    {
        _maxLaps = _positions.Max(p => p.Value.Count) - 1;
        SetLapInfoForStart();
    }

    private readonly Dictionary<int, List<int>> _positions = new()
    {
        { 18, [1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1] },
        { 5, [2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2] },
        { 10, [3, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 9, 7, 6, 6, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4] },
        { 9, [4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 1, 1, 2, 3, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3] },
        { 15, [5, 24, 24, 24, 24, 24, 24, 24, 24, 24, 22, 20, 20, 18, 16, 16, 17, 18, 19, 19, 18, 18, 18, 18, 18, 17, 17, 17, 16, 16, 15, 15, 15, 15, 15, 15, 16, 16, 99] },
        { 8, [6, 4, 4, 4, 4, 4, 4, 4, 4, 5, 13, 8, 6, 5, 5, 4, 5, 5, 5, 5, 5, 5, 7, 13, 11, 10, 9, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 5, 7, 7, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7] },
        { 1, [7, 7, 7, 7, 7, 7, 7, 9, 18, 17, 14, 11, 10, 7, 7, 6, 6, 6, 6, 6, 6, 6, 5, 5, 5, 5, 5, 4, 6, 6, 6, 9, 9, 9, 9, 9, 8, 8, 7, 7, 6, 5, 5, 10, 10, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 8, 8, 7, 6, 6, 6] },
        { 7, [8, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 10, 9, 99] },
        { 14, [9, 9, 9, 9, 9, 9, 9, 8, 8, 19, 17, 13, 12, 9, 9, 8, 8, 8, 8, 8, 8, 8, 8, 7, 7, 7, 6, 10, 10, 9, 9, 8, 8, 7, 7, 6, 6, 6, 6, 6, 5, 6, 7, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, 5, 5, 5, 5, 5] },
        { 3, [10, 8, 8, 8, 8, 8, 8, 7, 7, 7, 15, 12, 11, 8, 8, 7, 7, 7, 7, 7, 7, 7, 6, 6, 6, 6, 10, 9, 9, 8, 8, 7, 7, 8, 8, 7, 7, 7, 8, 14, 11, 9, 9, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9] },
        { 2, [11, 12, 12, 12, 12, 12, 14, 22, 20, 20, 18, 14, 13, 11, 10, 9, 9, 15, 17, 17, 17, 17, 16, 16, 14, 14, 13, 12, 12, 11, 11, 11, 11, 11, 11, 11, 10, 10, 10, 8, 9, 13, 13, 12, 12, 12, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11] },
        { 11, [12, 13, 13, 13, 13, 13, 12, 12, 11, 11, 20, 15, 14, 12, 11, 10, 10, 9, 9, 9, 9, 9, 9, 10, 15, 15, 14, 13, 13, 13, 12, 13, 13, 14, 14, 14, 13, 14, 14, 12, 12, 11, 11, 15, 15, 15, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14] },
        { 12, [13, 14, 14, 14, 14, 14, 13, 13, 12, 12, 10, 19, 17, 15, 14, 13, 13, 13, 13, 14, 16, 15, 15, 15, 13, 13, 12, 11, 11, 10, 10, 10, 10, 10, 10, 10, 9, 9, 9, 10, 14, 12, 12, 11, 11, 11, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10] },
        { 17, [14, 10, 10, 10, 10, 10, 10, 10, 9, 8, 7, 16, 15, 13, 12, 11, 11, 10, 10, 10, 10, 10, 10, 12, 16, 16, 15, 14, 14, 14, 13, 12, 12, 12, 12, 12, 11, 11, 11, 11, 13, 15, 15, 14, 14, 14, 13, 13, 13, 13, 13, 13, 13, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12] },
        { 16, [15, 15, 15, 15, 15, 15, 15, 15, 14, 13, 9, 6, 19, 16, 15, 15, 15, 14, 14, 13, 13, 13, 13, 11, 10, 11, 16, 15, 15, 15, 14, 14, 14, 13, 13, 13, 12, 12, 12, 15, 15, 14, 14, 13, 13, 13, 12, 12, 12, 12, 12, 12, 12, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13] },
        { 6, [16, 11, 11, 11, 11, 11, 11, 11, 10, 9, 8, 17, 16, 14, 13, 12, 12, 11, 11, 11, 11, 11, 11, 8, 8, 8, 7, 6, 7, 12, 16, 16, 16, 16, 16, 16, 15, 15, 15, 13, 10, 10, 10, 9, 9, 10, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15] },
        { 19, [17, 17, 17, 18, 18, 18, 18, 17, 16, 15, 12, 7, 8, 99] },
        { 21, [18, 18, 19, 19, 19, 19, 19, 18, 17, 16, 16, 22, 22, 20, 20, 18, 18, 17, 16, 16, 15, 16, 17, 17, 17, 18, 18, 18, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 17, 17, 17, 17, 17, 17, 16, 18, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17] },
        { 20, [19, 16, 16, 16, 16, 17, 17, 16, 15, 14, 11, 5, 5, 10, 17, 17, 16, 16, 15, 15, 14, 14, 14, 14, 12, 12, 11, 16, 18, 17, 17, 17, 17, 17, 17, 17, 17, 17, 16, 16, 16, 16, 16, 16, 17, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16] },
        { 25, [20, 21, 23, 23, 23, 23, 23, 21, 22, 22, 21, 21, 21, 19, 19, 19, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 19, 20, 20, 20, 20, 20, 20, 20, 20, 99] },
        { 24, [21, 19, 20, 20, 20, 20, 20, 19, 19, 18, 19, 18, 18, 17, 18, 20, 19, 19, 18, 18, 19, 19, 19, 19, 19, 19, 19, 20, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 18, 18, 18, 18, 18, 18, 18, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18] },
        { 22, [22, 22, 21, 21, 21, 21, 21, 20, 21, 21, 23, 24, 23, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 20, 20, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19] },
        { 23, [23, 23, 22, 22, 22, 22, 22, 23, 23, 23, 24, 23, 24, 22, 22, 22, 22, 22, 22, 22, 21, 21, 21, 99] },
        { 4, [24, 20, 18, 17, 17, 16, 16, 14, 13, 10, 6, 4, 4, 4, 4, 14, 14, 12, 12, 12, 12, 12, 12, 9, 9, 9, 8, 7, 5, 5, 5, 5, 5, 5, 5, 8, 14, 13, 13, 9, 8, 8, 8, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8] },
    };

    private void SetLapInfoForStart()
    {
        _lapInfo = [.. _positions.Select(x => new LapRacerInfo
        {
            Racer = _f1.Racers.Where(r => r.Number == x.Key).Single(),
            Lap = 0,
            Position = x.Value.First(),
            PositionChange = PositionChange.None
        })];
    }

    public IEnumerable<LapRacerInfo>? GetLapInfo() => _lapInfo;

    public bool NextLap()
    {
        _currentLap++;
        if (_currentLap > _maxLaps || _lapInfo is null) return false;

        foreach (var info in _lapInfo)
        {
            int lastPosition = info.Position;
            var racerInfo = _positions.First(x => x.Key == info.Racer.Number);

            if (racerInfo.Value.Count > _currentLap)
            {
                info.Position = racerInfo.Value[_currentLap];
            }
            else
            {
                info.Position = lastPosition;
            }
            info.PositionChange = GetPositionChange(lastPosition, info.Position);

            info.Lap = _currentLap;
        }
        return true;
    }
    private static PositionChange GetPositionChange(int oldPosition, int newPosition) =>
        (oldPosition, newPosition) switch
        {
            (PositionOut, _) => PositionChange.Out,
            (_, PositionOut) => PositionChange.Out,
            (var x, var y) when x == y => PositionChange.None,
            (var x, var y) when x < y => PositionChange.Down,
            _ => PositionChange.Up
        };
}
