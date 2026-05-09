using Tams.Api.Models;
using Tams.Api.Services.Warranty;

namespace Tams.Api.Tests;

public class WarrantyServiceTests
{
    private static WarrantyService CreateService() => new(null!, null!);

    // =========== ComputeEndDate ===========

    [Fact]
    public void ComputeEndDate_AddsTermMonthsToStartDate()
    {
        var service = CreateService();
        var start = new DateOnly(2024, 1, 15);

        var result = service.ComputeEndDate(start, 12);

        Assert.Equal(new DateOnly(2025, 1, 15), result);
    }

    [Fact]
    public void ComputeEndDate_HandlesEndOfMonthRollover()
    {
        var service = CreateService();
        var start = new DateOnly(2024, 1, 31);

        var result = service.ComputeEndDate(start, 1);

        Assert.Equal(new DateOnly(2024, 2, 29), result);
    }

    [Fact]
    public void ComputeEndDate_ZeroMonths_ReturnsStartDate()
    {
        var service = CreateService();
        var start = new DateOnly(2024, 6, 1);

        var result = service.ComputeEndDate(start, 0);

        Assert.Equal(start, result);
    }

    [Fact]
    public void ComputeEndDate_DefaultStart_Throws()
    {
        var service = CreateService();
        Assert.Throws<ArgumentException>(() => service.ComputeEndDate(default, 12));
    }

    // =========== ComputeWarrantyStatus ===========

    [Fact]
    public void ComputeWarrantyStatus_NullEndDate_ReturnsNull()
    {
        var service = CreateService();
        var warranty = new ItemWarranty { WarrantyEndDate = null };

        Assert.Null(service.ComputeWarrantyStatus(warranty));
    }

    [Fact]
    public void ComputeWarrantyStatus_EndDateInPast_ReturnsExpired()
    {
        var service = CreateService();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var warranty = new ItemWarranty { WarrantyEndDate = today.AddDays(-1) };

        Assert.Equal("Expired", service.ComputeWarrantyStatus(warranty));
    }

    [Fact]
    public void ComputeWarrantyStatus_EndDateWithin30Days_ReturnsExpiringSoon()
    {
        var service = CreateService();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var warranty = new ItemWarranty { WarrantyEndDate = today.AddDays(15) };

        Assert.Equal("Expiring Soon", service.ComputeWarrantyStatus(warranty));
    }

    [Fact]
    public void ComputeWarrantyStatus_EndDateExactly30Days_ReturnsExpiringSoon()
    {
        var service = CreateService();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var warranty = new ItemWarranty { WarrantyEndDate = today.AddDays(30) };

        Assert.Equal("Expiring Soon", service.ComputeWarrantyStatus(warranty));
    }

    [Fact]
    public void ComputeWarrantyStatus_EndDateBeyond30Days_ReturnsActive()
    {
        var service = CreateService();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var warranty = new ItemWarranty { WarrantyEndDate = today.AddDays(60) };

        Assert.Equal("Active", service.ComputeWarrantyStatus(warranty));
    }
}
