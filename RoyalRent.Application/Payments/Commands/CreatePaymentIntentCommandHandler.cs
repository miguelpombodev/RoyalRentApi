using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Payments.Commands;

public class CreatePaymentIntentCommandHandler : ICommandHandler<CreatePaymentIntentCommand, Result<string>>
{
    public Task<Result<string>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
