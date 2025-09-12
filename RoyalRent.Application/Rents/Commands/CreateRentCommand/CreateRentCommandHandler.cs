using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Cars.Errors;
using RoyalRent.Domain.Cars.Interfaces;
using RoyalRent.Domain.Payments.Entities;
using RoyalRent.Domain.Payments.Interfaces;
using RoyalRent.Domain.Rents.Services;

namespace RoyalRent.Application.Rents.Commands.CreateRentCommand;

public class CreateRentCommandHandler : ICommandHandler<CreateRentCommand, Result<IntentPaymentsInformation>>
{
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly ICarsRepository _carsRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<CreateRentCommandHandler> _logger;
    private readonly IRentDomainServices _rentDomainServices;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRentCommandHandler(IPaymentProcessor paymentProcessor, ICarsRepository carsRepository,
        ILogger<CreateRentCommandHandler> logger, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IRentDomainServices rentDomainServices)
    {
        _paymentProcessor = paymentProcessor;
        _carsRepository = carsRepository;
        _logger = logger;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _rentDomainServices = rentDomainServices;
    }

    public async Task<Result<IntentPaymentsInformation>> Handle(CreateRentCommand command,
        CancellationToken cancellationToken)
    {
        var car = await _carsRepository.GetCarById(command.CarId);

        if (car is null)
        {
            _logger.LogWarning("Tried to find car with id {RequestedCarId} but there's no data for the mentioned id",
                command.CarId);
            return Result<IntentPaymentsInformation>.Failure(CarsErrors.CarNotFound);
        }

        var rent = _rentDomainServices.CreateRent(command.UserId, car, command.StartDate,
            command.EndDate);

        _logger.LogInformation(
            "Rent amount was calculated - Base Value: {RentBaseAmount} , Value with fee: {RentAmountWithFee}",
            rent.Amount, rent.AmountWithFee);

        var intentPaymentsInformation =
            await _paymentProcessor.CreatePaymentAndRetrieveClientSecret(rent.AmountWithFee, rent.Id, car.Id,
                rent.UserId);

        var getOrCreateNewPaymentStatusResult =
            await _paymentRepository.GetByNameOrCreatePaymentStatus(intentPaymentsInformation.GetPaymentStatus());

        // Add a factory pattern here
        var payment = new Payment(command.UserId, getOrCreateNewPaymentStatusResult.Id);

        await _paymentRepository.Create(payment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<IntentPaymentsInformation>.Success(intentPaymentsInformation);
    }
}
