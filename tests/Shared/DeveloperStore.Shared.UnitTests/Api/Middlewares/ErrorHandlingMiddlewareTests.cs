using System.Net;
using System.Text.Json;
using DeveloperStore.Shared.Api.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DeveloperStore.Shared.UnitTests.Api.Middlewares;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task Should_ForwardRequest_WhenNoException()
    {
        var context = new DefaultHttpContext();
        var called = false;

        RequestDelegate next = ctx =>
        {
            called = true;
            return Task.CompletedTask;
        };

        var logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        await middleware.Invoke(context);

        called.Should().BeTrue();
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK); // default status
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_WhenUnhandledExceptionOccurs()
    {
        var context = new DefaultHttpContext();
        var exceptionMessage = "Unexpected failure in pipeline";

        RequestDelegate next = ctx => throw new InvalidOperationException(exceptionMessage);
        var logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        var stream = new MemoryStream();
        context.Response.Body = stream;

        await middleware.Invoke(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        stream.Position = 0;
        var body = await new StreamReader(stream).ReadToEndAsync();

        var json = JsonSerializer.Deserialize<JsonElement>(body);

        json.GetProperty("type").GetString().Should().Be("InternalServerError");
        json.GetProperty("error").GetString().Should().Be("Unexpected failure");
        json.GetProperty("detail").GetString().Should().Be(exceptionMessage);

        logger
            .When(l => l.Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()
            ))
            .Do(call =>
            {
                var exception = (Exception)call.Args()[3];
                var formatter = (Func<object, Exception, string>)call.Args()[4];
                var message = formatter(call.Args()[2], exception);

                message.Should().Contain("Unhandled exception");
                exception.Message.Should().Be("Unexpected failure in pipeline");
            });
    }
}