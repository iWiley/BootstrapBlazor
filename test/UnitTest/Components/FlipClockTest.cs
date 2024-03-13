﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace UnitTest.Components;

public class FlipClockTest : BootstrapBlazorTestBase
{
    [Fact]
    public void ShowHour_Ok()
    {
        var cut = Context.RenderComponent<FlipClock>();
        cut.Contains("bb-flip-clock-list hour");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.ShowHour, false);
        });
        cut.DoesNotContain("bb-flip-clock-list hour");
    }

    [Fact]
    public void ShowMinute_Ok()
    {
        var cut = Context.RenderComponent<FlipClock>();
        cut.Contains("bb-flip-clock-list minute");

        cut.SetParametersAndRender(pb =>
        {
            pb.Add(a => a.ShowMinute, false);
            pb.Add(a => a.UseLocaleTimeZone, true);
        });
        cut.DoesNotContain("bb-flip-clock-list minute");
    }
}
