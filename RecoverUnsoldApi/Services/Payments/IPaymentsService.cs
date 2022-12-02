namespace RecoverUnsoldApi.Services.Payments;

public interface IPaymentsService
{
    Task CreatePayment(Guid orderId, string transactionId);
    Task<bool> CheckPaymentSuccess(string transactionId);
}