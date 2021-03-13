// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeZoneList
{
  using System;
  using System.Runtime.InteropServices;
  using System.Text.Json;
  using System.Text.Json.Serialization;
  using TimeZoneConverter;

  /// <summary>
  /// Add <see cref="TimeZoneJsonConverter.Instance"/> to your serializer
  /// settings to serialize and deserialize <see cref="TimeZoneInfo"/> objects.
  /// </summary>
  public class TimeZoneJsonConverter : JsonConverter<TimeZoneInfo>
  {
    private static readonly bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets an allocation-free instance of the <see
    /// cref="TimeZoneJsonConverter"/>.
    /// </summary>
    public static TimeZoneJsonConverter Instance { get; } = new();

    /// <inheritdoc/>
    public override TimeZoneInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      var tzId = reader.GetString();
      reader.Read();
      return TZConvert.GetTimeZoneInfo(tzId);
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TimeZoneInfo value, JsonSerializerOptions options)
    {
      var id = _isWindows ? value.Id : TZConvert.IanaToWindows(value.Id);
      writer.WriteStringValue(id);
    }
  }
}
