using System.Globalization;
using DeveloperStore.Shared.Api.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace DeveloperStore.Shared.UnitTests.Api.Queries;

public class RequestQueryOptionsBinderTests
{
    private static async Task<RequestQueryOptions> BindAsync(Dictionary<string, StringValues> queryParams)
    {
        var binder = new RequestQueryOptionsBinder();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Query = new QueryCollection(queryParams);

        var bindingContext = new DefaultModelBindingContext();
        bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(RequestQueryOptions));
        bindingContext.ModelState = new ModelStateDictionary();
        bindingContext.ValueProvider = new QueryStringValueProvider(
            BindingSource.Query, 
            httpContext.Request.Query, 
            CultureInfo.InvariantCulture);

        bindingContext.ActionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new RouteData()
        };

        await binder.BindModelAsync(bindingContext);
        return bindingContext.Result.Model as RequestQueryOptions;
    }

    [Fact]
    public async Task Should_Bind_DefaultValues_When_QueryIsEmpty()
    {
        var result = await BindAsync(new Dictionary<string, StringValues>());

        result.Page.Should().Be(1);
        result.Size.Should().Be(10);
        result.Order.Should().BeNull();
        result.Filters.Should().BeEmpty();
        result.MinValues.Should().BeEmpty();
        result.MaxValues.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Parse_Page_Size_Order()
    {
        var result = await BindAsync(new()
        {
            ["_page"] = "2",
            ["_size"] = "50",
            ["_order"] = "title"
        });

        result.Page.Should().Be(2);
        result.Size.Should().Be(50);
        result.Order.Should().Be("title");
    }

    [Fact]
    public async Task Should_Parse_Filters_WithoutPrefix()
    {
        var result = await BindAsync(new()
        {
            ["category"] = "games",
            ["brand"] = "nintendo"
        });

        result.Filters.Should().ContainKey("category").And.ContainKey("brand");
        result.Filters["category"].Should().Be("games");
        result.Filters["brand"].Should().Be("nintendo");
    }

    [Fact]
    public async Task Should_Parse_MinAndMaxValues()
    {
        var result = await BindAsync(new()
        {
            ["_minprice"] = "100",
            ["_maxprice"] = "300",
            ["_minrating"] = "4.5"
        });

        result.MinValues.Should().ContainKey("price").And.ContainKey("rating");
        result.MaxValues.Should().ContainKey("price");

        result.MinValues["price"].Should().Be(100);
        result.MinValues["rating"].Should().Be(4.5m);
        result.MaxValues["price"].Should().Be(300);
    }
}