using RecoverUnsoldApi.Services.Auth;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Dto;

public record UserDataDto(Guid Id, string Username, string Email, string? FirstName, string? LastName,
    string? Phone, string? TaxId, string? Rccm, string? WebsiteUrl,
    string Role, DateTime? EmailVerifiedAt, DateTime CreatedAt)
{
    public static UserDataDto FromCustomer(Customer customer)
    {
        return new UserDataDto(
            customer.Id,
            customer.Username,
            customer.Email,
            customer.FirstName,
            customer.LastName,
            null,
            null,
            null,
            null,
            Roles.Customer,
            customer.EmailVerifiedAt,
            customer.CreatedAt
        );
    }
    
    public static UserDataDto FromDistributor(Distributor distributor)
    {
        return new UserDataDto(
            distributor.Id,
            distributor.Username,
            distributor.Email,
            null,
            null,
            distributor.Phone,
            distributor.TaxId,
            distributor.Rccm,
            distributor.WebsiteUrl,
            Roles.Distributor,
            distributor.EmailVerifiedAt,
            distributor.CreatedAt
        );
    }
}