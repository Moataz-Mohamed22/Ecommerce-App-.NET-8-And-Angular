using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

public class CustomerCartRepository : ICustomerCartRepository
{
    private readonly IDatabase _db;

    public CustomerCartRepository(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<bool> DeleteCartAsync(string customerId)
    {
        return await _db.KeyDeleteAsync(customerId);
    }

    public async Task<CustomerCart?> GetCartAsync(string customerId)
    {
        var result = await _db.StringGetAsync(customerId);

        if (result.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CustomerCart>(result!);
    }

    public async Task<CustomerCart?> UpdatedCartAsync(CustomerCart cart)
    {
        var result = await _db.StringSetAsync(
            cart.Id,
            JsonSerializer.Serialize(cart),
            TimeSpan.FromDays(3)
        );

        if (!result)
            return null;

        return await GetCartAsync(cart.Id);
    }
}