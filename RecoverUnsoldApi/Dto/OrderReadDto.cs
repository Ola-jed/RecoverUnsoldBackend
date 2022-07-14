using RecoverUnsoldApi.Entities.Enums;

namespace RecoverUnsoldApi.Dto;

public record OrderReadDto(Guid Id, DateTime WithdrawalDate, CustomerReadDto Customer, OfferReadDto Offer,
    Status Status, IEnumerable<OpinionReadDto> Opinions);