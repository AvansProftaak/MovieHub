using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class PaymentControllerTests
{
    private readonly PaymentsController _controller;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public PaymentControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("PaymentTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new PaymentsController(_hostEnvironment, _context);
    }
    
    [Fact]
    public void Test_Update_Status_0_Returns_Document()
    {
        _context.Database.EnsureDeleted();
        InsertTestData(_context);
        
        var result = _controller.UpdateStatus(1, 0);
        Assert.IsType<Task<IActionResult>>(result);
        
        Assert.Equal(StatusEnum.Open, _context.Payment.FirstOrDefault(p => p.Id == 1).Status);
        
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public void Test_Update_Status_1_Applies_To_Payment()
    {
        _context.Database.EnsureDeleted();
        InsertTestData(_context);
        
        var result = _controller.UpdateStatus(1, 1);
        Assert.IsType<Task<IActionResult>>(result);
        
        Assert.Equal(StatusEnum.Pending, _context.Payment.FirstOrDefault(p => p.Id == 1).Status);

        _context.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Test_Update_Status_2_Applies_To_Payment()
    {
        _context.Database.EnsureDeleted();
        InsertTestData(_context);
        
        var result = _controller.UpdateStatus(1, 2);
        Assert.IsType<Task<IActionResult>>(result);
        
        Assert.Equal(StatusEnum.Paid, _context.Payment.FirstOrDefault(p => p.Id == 1).Status);
        _context.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Test_Update_Status_3_Applies_To_Payment()
    {
        _context.Database.EnsureDeleted();
        InsertTestData(_context);
        
        var result = _controller.UpdateStatus(1, 3);
        Assert.IsType<Task<IActionResult>>(result);
        
        Assert.Equal(StatusEnum.Open, _context.Payment.FirstOrDefault(p => p.Id == 1).Status);
        
        _context.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Test_Apply_Voucher_Applies_Discount()
    {
        _context.Database.EnsureDeleted();
        InsertTestData(_context);
        
        ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

        _controller.TempData = tempData;

        var barCode = _context.Vouchers.FirstOrDefault(v => v.Id == 1).Barcode;

        _controller.ApplyCoupon(1, 1, barCode);

        Assert.Equal((decimal)6.5, _context.Order.FirstOrDefault(p => p.Id == 1).TotalPrice);
        _context.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Test_Apply_TenRides_Applies_Discount()
    {
        _context.Database.EnsureDeleted();
        
        ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
        
        InsertTestData(_context);
        
        _controller.TempData = tempData;

        var barCode = _context.Vouchers.FirstOrDefault(v => v.Id == 2).Barcode;

        _controller.ApplyCoupon(1, 1, barCode);

        Assert.Equal(3, _context.Order.FirstOrDefault(p => p.Id == 1).TotalPrice);
        Assert.Equal(5, _context.Vouchers.FirstOrDefault(p => p.Id == 2).Movies);
        
        _context.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Test_Apply_Subscription_Applies_Discount()
    {
        _context.Database.EnsureDeleted();
        
        ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
        TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
        
        InsertTestData(_context);
        
        _controller.TempData = tempData;

        var barCode = _context.Vouchers.FirstOrDefault(v => v.Id == 3).Barcode;

        _controller.ApplyCoupon(1, 1, barCode);

        Assert.Equal(3, _context.Order.FirstOrDefault(p => p.Id == 1).TotalPrice);
        
        _context.Database.EnsureDeleted();
    }

    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new User()
        {
            Id = 1,
            FirstName = "Joris",
            LastName = "Jansen",
            PhoneNumber = 0612345678,
            Email = "joris97jansen@gmail.com",
            AcceptedNewsletter = true
        });

        context.Add(new Order()
        {
            Id = 1,
            UserId = 1,
            ShowtimeId = 1,
            TotalPrice = (decimal)11.5
        });

        context.Add(new Voucher()
        {
            Id = 1,
            Name = "Voucher",
            Price = (decimal) 5.0,
            paid = true,
            Barcode = Guid.NewGuid().ToString(),
            FirstName = "Joris",
            LastName = "Jansen"
        });
        
        context.Add(new Voucher()
        {
            Id = 2,
            Name = "TenRides",
            Price = 70,
            paid = true,
            Movies = 6,
            Barcode = Guid.NewGuid().ToString(),
            FirstName = "Joris",
            LastName = "Jansen"
        });
        
        context.Add(new Voucher()
        {
            Id = 3,
            Name = "Subscription",
            Price = 17,
            paid = true,
            Movies = 0,
            Barcode = Guid.NewGuid().ToString(),
            FirstName = "Joris",
            LastName = "Jansen"
        });
        
        context.Add(new Payment()
        {
            Id = 1,
            Status = StatusEnum.Open,
            OrderId = 1,
            PaymentMethodId = 1,
        });
        
        context.Add(new PaymentMethod()
        {
            Id = 1,
            Name = "Credit Card",
            Description = "cc"
        });
        
        context.Add(new PaymentMethod()
        {
            Id = 2,
            Name = "PayPal",
            Description = "pp"
        });
        
        context.Add(new PaymentMethod()
        {
            Id = 3,
            Name = "iDeal",
            Description = "ideaal"
        });
        
        context.SaveChanges();
    }
}