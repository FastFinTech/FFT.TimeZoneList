// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Generator
{
  using System;
  using System.Diagnostics;
  using System.Linq;
  using Microsoft.CodeAnalysis;

  [Generator]
  public class TimeZoneListGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
      if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.DebugSourceGenerators", out var debugValue) &&
          bool.TryParse(debugValue, out var shouldDebug) &&
          shouldDebug)
      {
        Debugger.Launch();
      }

      var result = @"namespace FFT.TimeZoneList
{
  using System;
  using TimeZoneConverter;

  /// <summary>
  /// Contains hard-coded TimeZoneInfo references.
  /// </summary>
  public sealed class TimeZones
  {
[items]

    private static TimeZoneInfo? Get(string id)
    {
       try
       {
         return TZConvert.GetTimeZoneInfo(id)!;
       }
       catch
       {
         return null;
       }
    }
  }
}
";

      var items = TimeZoneInfo.GetSystemTimeZones().Select(tz =>
      {
        var name = tz.Id
            .Replace(" ", string.Empty)
            .Replace(".", string.Empty)
            .Replace("(", "_")
            .Replace("+", "_PLUS_")
            .Replace("-", "_MINUS_")
            .Replace(")", "_");

        while (name.EndsWith("_"))
          name = name.Substring(0, name.Length - 1);

        var doc = $@"    /// <summary>
    /// Gets the timezone: {tz.ToString()}. [{tz.Id}]{(tz.SupportsDaylightSavingTime ? " (Supports Daylight Saving Time)" : string.Empty)}.
    /// Returns <c>null</c> if the timezone is not installed on the current system.
    /// </summary>";

        doc = doc.Replace("&", "and");

        var property = $@"    public static TimeZoneInfo? {name} {{ get; }} = Get(""{tz.Id}"");";

        return new
        {
          doc,
          property,
        };
      })
        .OrderBy(x => x.property)
        .Select(x => x.doc + Environment.NewLine + x.property);

      result = result.Replace("[items]", string.Join(Environment.NewLine + Environment.NewLine, items));

      context.AddSource("TimeZones.cs", result);
    }
  }
}
