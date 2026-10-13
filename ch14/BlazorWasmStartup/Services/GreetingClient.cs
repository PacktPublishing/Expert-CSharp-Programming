// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Options;

namespace BlazorWasmStartup.Services;

interface IGreetingClient
{
    Task<string> GetGreetingAsync(CancellationToken cancellationToken = default);
}

sealed class GreetingClient(
    HttpClient httpClient,
    IOptions<ClientAppOptions> options,
    ILogger<GreetingClient> logger) : IGreetingClient
{
    public Task<string> GetGreetingAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Preparing greeting with base address {BaseAddress}",
            httpClient.BaseAddress);

        string message = $"Hello from {options.Value.Name}. Base address: {httpClient.BaseAddress}";

        return Task.FromResult(message);
    }
}

sealed class ClientAppOptions
{
    [Required]
    public string Name { get; init; } = "BlazorWasmStartup";

    [Required]
    public string Version { get; init; } = "1.0.0";

    [Range(1, 300)]
    public int RefreshSeconds { get; init; } = 10;
}
