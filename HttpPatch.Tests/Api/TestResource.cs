using System.Collections.Immutable;
using HttpPatch.Tests.Api.Controllers;

namespace HttpPatch.Tests.Api;

public record TestResource(string Id, string Name, string? Description, int Count, Address Address, ResourceState State, ImmutableList<string> People);