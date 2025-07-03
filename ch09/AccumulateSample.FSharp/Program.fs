// F# equivalent of the C# AccumulateSample

open System
open System.Numerics

// Define an array of numbers
let numbers = [| 1; 3; 5; 11; 22; 33; 42; 44; 46 |]

// Basic accumulate function
let accumulate (values: int[]) =
    let mutable sum = 0
    for n in values do
        sum <- sum + n
    sum

// Generic accumulate function
let accumulateGeneric<'T when 'T :> INumber<'T>> (values: 'T[]) =
    let mutable sum = 'T.Zero
    for n in values do
        sum <- sum + n
    sum

// Recursive accumulate function
let rec accumulateRecursive<'T when 'T :> INumber<'T>> (values: 'T[]) =
    match values with
    | [| |] -> 'T.Zero
    | [| first |] -> first
    | _ -> 
        let first = values[0]
        let rest = values[1..]
        first + (accumulateRecursive rest)

// Recursive accumulate function with Span
let rec accumulateRecursiveSpan<'T when 'T :> INumber<'T>> (values: ReadOnlySpan<'T>) =
    match values.Length with
    | 0 -> 'T.Zero
    | _ -> 
        let first = values[0]
        let rest = values.Slice(1)
        first + (accumulateRecursiveSpan rest)

// Execute and display results
let result = accumulate numbers
printfn "%d" result

let genericResult = accumulateGeneric numbers
printfn "Using generic %d" genericResult

let recursiveResult = accumulateRecursive numbers
printfn "Using recursive %d" recursiveResult

let recursiveSpanResult = accumulateRecursiveSpan (ReadOnlySpan(numbers))
printfn "Using recursive with Span %d" recursiveSpanResult

// Uncommented benchmarking code (similar to the C# version)

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs

[<SimpleJob(RuntimeMoniker.Net80)>]
[<SimpleJob(RuntimeMoniker.Net90)>]
[<MemoryDiagnoser>]
type BenchmarkAccumulate() =
    let _data = [| 1..100 |]
    
    [<Benchmark(Baseline = true)>]
    member _.AccumulateFor() = BenchmarkAccumulate.AccumulateFor(_data)
    
    [<Benchmark>]
    member _.AccumulateForeach() = accumulate _data
    
    [<Benchmark>]
    member _.AccumulateGeneric() = accumulateGeneric _data
    
    [<Benchmark>]
    member _.AccumulateRecursive() = accumulateRecursive _data
    
    [<Benchmark>]
    member _.AccumulateRecursiveSpan() = accumulateRecursiveSpan (ReadOnlySpan(_data))
    
    static member AccumulateFor(values: int[]) =
        let mutable sum = 0
        for i = 0 to values.Length - 1 do
            sum <- sum + values[i]
        sum


// Comment out the benchmarking execution (similar to C# version)
BenchmarkRunner.Run<BenchmarkAccumulate>() |> ignore
