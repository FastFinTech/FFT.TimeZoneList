// Copyright (c) True Goodwill. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FFT.TimeZoneList.Tests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using TimeZoneConverter;

  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      Assert.IsNotNull(TimeZones.EasternStandardTime);
    }
  }
}
