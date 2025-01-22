| Method        | Job      | Runtime  | Mean         | Error      | StdDev     | Gen0   | Allocated |
|-------------- |--------- |--------- |-------------:|-----------:|-----------:|-------:|----------:|
| ThrowAndCatch | .NET 6.0 | .NET 6.0 | 4,252.369 ns | 52.1502 ns | 46.2298 ns | 0.0229 |     344 B |
| StatusCode    | .NET 6.0 | .NET 6.0 |     6.000 ns |  0.0439 ns |  0.0410 ns |      - |         - |
| ThrowAndCatch | .NET 7.0 | .NET 7.0 | 4,496.957 ns | 14.9814 ns | 14.0136 ns | 0.0381 |     504 B |
| StatusCode    | .NET 7.0 | .NET 7.0 |     6.278 ns |  0.0308 ns |  0.0288 ns |      - |         - |
| ThrowAndCatch | .NET 8.0 | .NET 8.0 | 4,485.978 ns |  8.8347 ns |  7.8318 ns | 0.0381 |     504 B |
| StatusCode    | .NET 8.0 | .NET 8.0 |     2.988 ns |  0.0485 ns |  0.0430 ns |      - |         - |
| ThrowAndCatch | .NET 9.0 | .NET 9.0 | 2,802.647 ns | 19.3786 ns | 16.1820 ns | 0.0381 |     480 B |
| StatusCode    | .NET 9.0 | .NET 9.0 |     2.601 ns |  0.0202 ns |  0.0169 ns |      - |         - |