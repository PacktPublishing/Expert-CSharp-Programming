﻿using Microsoft.Extensions.Logging;

namespace CodeBreaker.Bot;

public static partial class Log
{
    [LoggerMessage(
        EventId = 3000,
        Level = LogLevel.Error,
        Message = "{Message}")]
    public static partial void Error(this ILogger logger, Exception ex, string message);

    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "Sending the move {Move} to {Game}")]
    public static partial void SendMove(this ILogger logger, string move, string game);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "Matched after {Number} moves with {Game}")]
    public static partial void Matched(this ILogger logger, int number, string game);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Information,
        Message = "Reduced the possible values to {Number} with {Color} hits in {game}")]
    public static partial void ReducedPossibleValues(this ILogger logger, int number, string color, string game);

    [LoggerMessage(
        EventId = 4003,
        Level = LogLevel.Information,
        Message = "Finished game with move {Number} in {Game}")]
    public static partial void FinishedGame(this ILogger logger, int number, string game);

    [LoggerMessage(
        EventId = 4004,
        Level = LogLevel.Information,
        Message = "Using URI {Uri} to access the API service")]
    public static partial void UsingUri(this ILogger logger, string uri);

    [LoggerMessage(
        EventId = 4005,
        Level = LogLevel.Information,
        Message = "Start CodeBreakerGameRunner")]
    public static partial void StartGameRunner(this ILogger logger);

    [LoggerMessage(
        EventId = 4006,
        Level = LogLevel.Trace,
        Message = "Waiting for next timer tick in loop {Loop}")]
    public static partial void WaitingForNextTick(this ILogger logger, int loop);

    [LoggerMessage(
        EventId = 4007,
        Level = LogLevel.Trace,
        Message = "Timer tick fired in loop {Loop}")]
    public static partial void TimerTickFired(this ILogger logger, int loop);

    [LoggerMessage(
        EventId = 4008,
        Level = LogLevel.Information,
        Message = "Received the result {Result} for move {Number} to {Game}")]
    public static partial void ReceivedResult(this ILogger logger, string result, int number, string game);

    [LoggerMessage(
        EventId = 4009,
        Level = LogLevel.Information,
        Message = "Ended after {Number} moves in {Game}")]
    public static partial void Ended(this ILogger logger, int number, string game);
}
