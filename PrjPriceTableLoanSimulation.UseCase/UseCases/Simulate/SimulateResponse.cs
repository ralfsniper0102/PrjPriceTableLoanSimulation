namespace PrjPriceTableLoanSimulation.UseCase.UseCases.Simulate
{
    public class SimulateResponse
    {
        public decimal MontlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public List<PaymentSchedule> PaymentSchedule { get; set; }
    }

    public class PaymentSchedule
    {
        public int Month { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }

    }
}
