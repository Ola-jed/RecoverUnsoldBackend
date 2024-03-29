﻿using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Dto;

public record OrderReadDto(Guid Id, DateTime WithdrawalDate, CustomerReadDto? Customer, PaymentDto? Payment,
    Guid OfferId, OfferReadDto? Offer, Status Status, IEnumerable<OpinionReadDto> Opinions, DateTime CreatedAt);