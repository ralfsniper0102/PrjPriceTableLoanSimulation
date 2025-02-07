using PrjPriceTableLoanSimulation.Domain.Bases;

namespace PrjPriceTableLoanSimulation.Domain.Entities
{
    public class PaymentSchedule : BaseEntity<int>
    {
        public int ProposalId { get; set; } 
        public Proposal Proposal { get; set; }

        public int Month { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal Balance { get; set; }
    }
}
