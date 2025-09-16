using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Rents.DTOs;
using RoyalRent.Domain.Payments.Entities;

namespace RoyalRent.Application.Rents.Commands.CreateRentCommand;

public sealed record CreateRentCommand(Guid CarId,
    DateTime StartDate,
    DateTime EndDate,
    Guid UserId) : ICommand<Result<RentPaymentData>>;
