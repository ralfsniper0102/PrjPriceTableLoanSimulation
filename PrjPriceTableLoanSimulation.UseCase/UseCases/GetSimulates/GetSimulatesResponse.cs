using PrjPriceTableLoanSimulation.Domain.Entities;

namespace PrjPriceTableLoanSimulation.UseCase.UseCases.GetSimulates
{
    public class GetSimulatesResponse
    {
        public List<SimulateViewModel> Simulates { get; set; }
        public int QtTotal { get; set; }
        public int QtPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class SimulateViewModel
    {
        public decimal LoanAmount { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public List<PaymentScheduleViewModel> PaymentSchedules { get; set; }
    }

    public class PaymentScheduleViewModel
    {
        public int Month { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }
}
