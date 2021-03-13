// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeZoneList.Tests
{
  using System;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using TimeZoneConverter;

  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
      {
        var name = Environment.OSVersion.Platform.ToString().StartsWith("Win")
          ? tz.Id
          : TZConvert.IanaToWindows(tz.Id);
      }

      Assert.IsNotNull(TimeZones.EasternStandardTime);
    }
  }
}
