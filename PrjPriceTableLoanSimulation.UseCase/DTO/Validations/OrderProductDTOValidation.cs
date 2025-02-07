using FluentValidation;

namespace PrjPriceTableLoanSimulation.UseCase.DTO.Validations;

public class OrderProductDTOValidation : AbstractValidator<OrderProductDTO>
{
    public OrderProductDTOValidation()
    {
        RuleFor(x => x.ProductId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThan(0);
    }
}