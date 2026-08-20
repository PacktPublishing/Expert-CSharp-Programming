// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Globalization;

namespace JsonSerialization.Models;

/// <summary>
/// A simple value-object for monetary amounts with ISO-4217 currency code.
/// Serialized as a compact string, e.g. "29.99 USD".
/// </summary>
public readonly record struct Money(decimal Amount, string Currency)
{
    public override string ToString()
        => $"{Amount.ToString("0.00", CultureInfo.InvariantCulture)} {Currency}";
}
