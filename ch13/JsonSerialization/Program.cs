// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Text;

using JsonSerialization;

Console.OutputEncoding = Encoding.UTF8;

Runner.BasicSerialization();
Runner.HierarchicalGraph();
Runner.SourceGeneratorSerialization();
Runner.GenericPagedResultSerialization();
await Runner.AsyncStreamingSerializationAsync();
Runner.PerformanceComparison();
Runner.JsonConverterErrorHandling();
