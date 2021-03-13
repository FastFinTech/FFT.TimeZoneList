// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Generator
{
  using System;
  using System.Diagnostics;
  using System.Linq;
  using System.Text;
  using Microsoft.CodeAnalysis;

  [Generator]
  public class TimeZoneListGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
#if DEBUG
      if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.DebugSourceGenerators", out var debugValue) &&
          bool.TryParse(debugValue, out var shouldDebug) &&
          shouldDebug)
      {
        Debugger.Launch();
      }
#endif

      var data = TimeZoneInfo.GetSystemTimeZones().Select(tz =>
      {
        var name = tz.Id
            .Replace(" ", string.Empty)
            .Replace(".", string.Empty)
            .Replace("(", "_")
            .Replace("+", "_PLUS_")
            .Replace("-", "_MINUS_")
            .Replace("/", "_")
            .Replace(")", "_");

        while (name.EndsWith("_"))
          name = name.Substring(0, name.Length - 1);

        var fieldName = "_" + char.ToLowerInvariant(name[0]) + name.Substring(1);

        var summary = $@"    /// <summary>
    /// Gets the timezone: {tz.ToString()}. [{tz.Id}]{(tz.SupportsDaylightSavingTime ? " (Supports Daylight Saving Time)" : string.Empty)}.
    /// </summary>
    /// <exception cref=""TimeZoneNotFoundException"">Thrown if the timezone is not installed on the current system.</exception>";

        summary = summary.Replace("&", "and");

        var property = $@"    public static TimeZoneInfo? {name} {{ get; }} = Get(""{tz.Id}"");";

        return new
        {
          id = tz.Id,
          name,
          fieldName,
          tz,
          summary,
          property,
        };
      }).ToList();

      var result = @"namespace FFT.TimeZoneList
{
  using System;
  using TimeZoneConverter;

  /// <summary>
  /// Contains hard-coded TimeZoneInfo references.
  /// </summary>
  public sealed class TimeZonesGen
  {
[fields]
[properties]
  }
}
";

      var fields = new StringBuilder();
      var properties = new StringBuilder();
      foreach (var x in data.OrderBy(d => d.name))
      {
        fields.AppendLine($@"    private static TimeZoneInfo? {x.fieldName} = null;");

        properties.AppendLine();
        properties.AppendLine(x.summary);
        properties.AppendLine($@"    public static TimeZoneInfo {x.name} => {x.fieldName} ??= TZConvert.GetTimeZoneInfo(""{x.id}"");");
      }

      result = result.Replace("[fields]", fields.ToString());
      result = result.Replace("[properties]", properties.ToString());
      context.AddSource("TimeZones.cs", result);
    }
  }
}
