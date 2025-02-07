using PrjPriceTableLoanSimulation.Domain.Bases;

namespace PrjPriceTableLoanSimulation.Domain.Entities
{
    public class Proposal : BaseEntity<int>
    {
        public decimal LoanAmount { get; set; }
        public decimal AnnualInterestRate { get; set; }
        public int NumberOfMonths { get; set; }
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }


        public List<PaymentSchedule> PaymentSchedules { get; set; } = new();
    }
}
