# Chapter 12 - Concurrency and Multithreading

This chapter requires the .NET 10 SDK to be installed.

The projects of this chapter are:
- ThreadPoolSamples: a console app demonstrating the ThreadPool class, using ThreadPool methods and properties to configure the .NET thread pool.
- ThreadsAndTasks: a console application to create threads via the Thread class and configure and use Task objects, and cooperative cancellation using CancellationToken.
- Synchronization: a console application demonstrating to use locks, lock-free scalar operations, throttling concurrency with SemaphoreSlim, and using multiple readers and one writer with ReaderWriterLockSlim.
- AsyncAwait: a console application showing an async pipeline, reducing memory allocations with ValueTask, avoiding context capturing with ConfigureAwait, and async streaming.
- BestPractices: a console application expanding on the previous concepts demonstrating parallel throttling with the Parallel class, using data parallelization with Parallel LINQ, a producer/consumer pipeline with the Channel class, and composing cancellation signals.
