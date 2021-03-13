# FFT.TimeZoneList

[![Source code](https://img.shields.io/static/v1?style=flat&label=&message=Source%20Code&logo=read-the-docs&color=informational)](https://github.com/FastFinTech/FFT.TimeZoneList)
[![NuGet
package](https://img.shields.io/nuget/v/FFT.TimeZoneList.svg)](https://nuget.org/packages/FFT.TimeZoneList)

Provides hard-typed references to timezones. Works cross-platform thanks to internal use of the `TimeZoneConverter` package.

```csharp
using FFT.TimeZoneList;
TimeZoneInfo _est = TimeZones.EasternStandardTime;
```

Also provides a Json converter for `System.Text.Json` serialization.

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using FFT.TimeZoneList;

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
options.Converters.Add(TimeZoneJsonConverter.Instance);
var json = @"{""TimeZone"":""Eastern Standard Time""}";
var value = JsonSerializer.Deserialize<MyDTO>(json, options);

public class MyDTO
{
  public TimeZoneInfo TimeZone { get; set; }
}
```
