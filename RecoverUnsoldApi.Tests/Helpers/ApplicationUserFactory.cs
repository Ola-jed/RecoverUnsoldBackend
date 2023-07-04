using Bogus;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Tests.Helpers;

public class ApplicationUserFactory
{
    private readonly DataContext _context;

    public ApplicationUserFactory(DataContext context)
    {
        _context = context;
    }

    public Customer GenerateCustomer()
    {
        return new Faker<Customer>()
            .StrictMode(true)
            .RuleFor(c => c.Username, faker => faker.Name.FullName())
            .RuleFor(c => c.Email, faker => faker.Internet.Email())
            .RuleFor(c => c.Password, faker => BCrypt.Net.BCrypt.HashPassword("password"))
            .RuleFor(c => c.EmailVerifiedAt, DateTime.Now)
            .RuleFor(c => c.FirstName, faker => faker.Name.FirstName())
            .RuleFor(c => c.LastName, faker => faker.Name.LastName())
            .Generate();
    }

    public async Task GenerateDistributor()
    {
    }
}