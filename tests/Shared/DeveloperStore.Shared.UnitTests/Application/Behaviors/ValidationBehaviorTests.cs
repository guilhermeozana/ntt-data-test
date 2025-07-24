using ErrorOr;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using DeveloperStore.Shared.Application.Behaviors;

namespace DeveloperStore.Shared.UnitTests.Application.Behaviors;

public class FakeRequest : IRequest<ErrorOr<string>>
{
    public string Input { get; set; } = string.Empty;
}

public class FakeRequestValidator : AbstractValidator<FakeRequest>
{
    public FakeRequestValidator()
    {
        RuleFor(x => x.Input)
            .NotEmpty().WithMessage("Input is required.");
    }
}

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_ReturnErrors_WhenRequestIsInvalid()
    {
        var validators = new List<IValidator<FakeRequest>> { new FakeRequestValidator() };

        var behavior = new ValidationBehavior<FakeRequest, ErrorOr<string>>(validators);
        var request = new FakeRequest { Input = "" };

        var next = Substitute.For<RequestHandlerDelegate<ErrorOr<string>>>();

        var result = await behavior.Handle(request, next, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be("Input");
    }

    [Fact]
    public async Task Should_ContinuePipeline_WhenRequestIsValid()
    {
        var validators = new List<IValidator<FakeRequest>> { new FakeRequestValidator() };

        var behavior = new ValidationBehavior<FakeRequest, ErrorOr<string>>(validators);
        var request = new FakeRequest { Input = "Hello" };

        var next = Substitute.For<RequestHandlerDelegate<ErrorOr<string>>>();
        next.Invoke().Returns("OK");

        var result = await behavior.Handle(request, next, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be("OK");
    }

    [Fact]
    public async Task Should_SkipValidation_WhenNoValidatorsRegistered()
    {
        var validators = new List<IValidator<FakeRequest>>();

        var behavior = new ValidationBehavior<FakeRequest, ErrorOr<string>>(validators);
        var request = new FakeRequest { Input = "" };

        var next = Substitute.For<RequestHandlerDelegate<ErrorOr<string>>>();
        next.Invoke().Returns("Skipped");

        var result = await behavior.Handle(request, next, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be("Skipped");
    }
}