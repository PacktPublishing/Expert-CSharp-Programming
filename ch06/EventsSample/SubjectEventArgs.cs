﻿namespace EventsSample;

public class SubjectEventArgs(int id) : EventArgs
{
    public int Id { get; } = id;
}
