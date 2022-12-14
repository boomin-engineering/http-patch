namespace HttpPatch.Tests.Api;

public class TestDataStore
{
    private readonly Dictionary<string, TestResource> _dataStore = new();
    
    public void UpsertResource(TestResource resource)
        => _dataStore[resource.Id] = resource;

    public TestResource GetResource(string resourceId) 
        => _dataStore.TryGetValue(resourceId, out var resource) ? resource : throw new InvalidOperationException($"The resource with id {resourceId} was not in the TestDataStore");
}