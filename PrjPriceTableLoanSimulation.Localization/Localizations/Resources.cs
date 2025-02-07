using Microsoft.Extensions.Localization;

namespace PrjPriceTableLoanSimulation.Localization.Localizations
{
    public class Resources
    {
        private readonly IStringLocalizer<Resources> _localizer;

        public Resources(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }

        public string ProposalNotExists() => _localizer[nameof(ProposalNotExists)];
    }
}
