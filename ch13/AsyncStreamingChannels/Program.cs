// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

Console.OutputEncoding = Encoding.UTF8;

// =====================================================
// System.Threading.Channels – fan-out pub/sub news feed
// =====================================================
Console.WriteLine("System.Threading.Channels – Fan-Out Pub/Sub News Feed");
Console.WriteLine("-----------------------------------------------------");

// Ingestion channel: two publishers write here (bounded, backpressure-aware)
var ingestChannel = Channel.CreateBounded<NewsArticle>(
    new BoundedChannelOptions(20)
    {
        FullMode = BoundedChannelFullMode.Wait,
        SingleWriter = false,   // two publishers
        SingleReader = true,    // one dispatcher reads and fans out
    });

// Per-subscriber channels (unbounded – dispatcher controls pacing)
var techChannel = Channel.CreateUnbounded<NewsArticle>();
var financeChannel = Channel.CreateUnbounded<NewsArticle>();

var ingestWriter = ingestChannel.Writer;

// Two publishers – simulating different news sources
Task publisher1 = PublishNewsAsync("Reuters", ingestWriter, 5, delay: 100);
Task publisher2 = PublishNewsAsync("Bloomberg", ingestWriter, 5, delay: 150);

// Dispatcher: routes each article to matching subscriber channels
Task dispatcher = DispatchNewsAsync(
    ingestChannel.Reader,
    new NewsSubscription("Tech", techChannel.Writer),
    new NewsSubscription("Finance", financeChannel.Writer));

// Two independent subscribers – each receives only its category stream
Task consumer1 = ConsumeNewsAsync("Subscriber-A [Tech]", techChannel.Reader);
Task consumer2 = ConsumeNewsAsync("Subscriber-B [Finance]", financeChannel.Reader);

// Wait for publishers, complete the ingestion channel, then drain everything
await Task.WhenAll(publisher1, publisher2);
ingestWriter.Complete();
await dispatcher;
await Task.WhenAll(consumer1, consumer2);
Console.WriteLine();

static async Task DispatchNewsAsync(
    ChannelReader<NewsArticle> source,
    params NewsSubscription[] subscribers)
{
    ArgumentNullException.ThrowIfNull(source);
    ArgumentNullException.ThrowIfNull(subscribers);

    try
    {
        // Topic routing: each article is forwarded only to matching subscribers
        await foreach (NewsArticle article in source.ReadAllAsync())
        {
            foreach (NewsSubscription subscriber in subscribers)
            {
                if (article.Category != subscriber.Category) continue;
                await subscriber.Writer.WriteAsync(article);
            }
        }
    }
    finally
    {
        foreach (NewsSubscription subscriber in subscribers)
            subscriber.Writer.Complete();
    }
}

static async Task PublishNewsAsync(
    string source,
    ChannelWriter<NewsArticle> channelWriter,
    int count,
    int delay)
{
    string[] categories = ["Tech", "Finance", "Sports", "Politics"];

    for (int i = 1; i <= count; i++)
    {
        NewsArticle article = new(
            Id: $"{source}-{i:000}",
            Headline: $"{source} headline #{i}",
            Category: categories[Random.Shared.Next(categories.Length)],
            PublishedAt: DateTimeOffset.UtcNow);

        await channelWriter.WriteAsync(article);
        Console.WriteLine($"Published [{article.Id}] {article.Headline} ({article.Category})");
        await Task.Delay(delay);
    }
}

static async Task ConsumeNewsAsync(
    string consumerName,
    ChannelReader<NewsArticle> channelReader)
{
    await foreach (NewsArticle article in channelReader.ReadAllAsync())
    {
        Console.WriteLine($" {consumerName}: [{article.Id}] {article.Headline}");
        await Task.Delay(20); // simulate processing
    }
}

// ==============================================
// Channels – performance: throughput measurement
// ==============================================
Console.WriteLine("Channel Throughput Benchmark");
Console.WriteLine("----------------------------");

const int MessageCount = 100_000;
Channel<int> benchChannel = Channel.CreateUnbounded<int>(
    new UnboundedChannelOptions { SingleWriter = true, SingleReader = true });

Stopwatch sw = Stopwatch.StartNew();

Task producer = Task.Run(async () =>
{
    for (int i = 0; i < MessageCount; i++)
        await benchChannel.Writer.WriteAsync(i);
    benchChannel.Writer.Complete();
});

int received = 0;
Task consumer = Task.Run(async () =>
{
    await foreach (int _ in benchChannel.Reader.ReadAllAsync())
        received++;
});

await Task.WhenAll(producer, consumer);
sw.Stop();

double throughputPerSec = MessageCount / (sw.Elapsed.TotalSeconds);
Console.WriteLine($"{MessageCount:N0} messages in {sw.ElapsedMilliseconds} ms");
Console.WriteLine($"Throughput: {throughputPerSec:N0} messages/sec");
Console.WriteLine();

// ============================================================
// Domain types
// ============================================================

sealed record StockTick(string Symbol, decimal Price, DateTimeOffset Timestamp);

sealed record NewsSubscription(string Category, ChannelWriter<NewsArticle> Writer);

sealed record NewsArticle(string Id, string Headline, string Category, DateTimeOffset PublishedAt);
