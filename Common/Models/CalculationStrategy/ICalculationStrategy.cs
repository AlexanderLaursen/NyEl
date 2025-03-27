namespace Common.Models.CalculationStrategy
{
    public interface ICalculationStrategy
    {
        decimal Calculate(CalculationParameters calculationParameters);
    }
}
