using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Rents.DTOs;

namespace RoyalRent.Application.Payments.Commands;

public record CreatePaymentIntentCommand(CreateRentRequest request) : ICommand<Result<string>>;
