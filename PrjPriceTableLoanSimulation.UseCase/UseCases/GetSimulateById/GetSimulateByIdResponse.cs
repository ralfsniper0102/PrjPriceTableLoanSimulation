using PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulateById
{
    public class GetSimulateByIdResponse
    {
        public decimal LoanAmount { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public List<PaymentScheduleViewModel> PaymentSchedules { get; set; }
    }
}
