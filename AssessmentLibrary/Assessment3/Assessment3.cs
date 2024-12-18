namespace AssessmentLibrary.Assessment3;
public class Assessment3
{
    public static decimal CalculateGrossAmount(decimal amount, decimal taxPercentage)
    {
        // Given a specified Nett amount, calculate the gross amount before tax deduction.

        //validations for input data are skipped
        return amount / (100 - taxPercentage) * 100;
    }

    public static decimal CalculateGrossAmount(decimal amount, decimal level1Threshold, decimal level1TaxPercentage,
        decimal level2TaxPercentage)
    {
        // Given a specified Nett amount, calculate the gross amount before tax deduction
        // Where the first level1Treshold gross amount is taxed at level1TaxPercentage 
        // and the balance is taxed at level2TaxPercentage.
        //
        // Eg: Gross of £325 is taxed as follows:
        //     First £125 taxed @ 20%
        //     Balance taxed @ 50%
        //
        // 20% of 125 = 25. 
        // 325 - 125 = 200.
        // 50% of 200 = 100.
        // Total Tax: 25 + 100 = 125
        // Nett (Gross - Tax): 325 - 125 = 200

        //validations for input data are skipped
        var tax1 = level1Threshold / 100 * level1TaxPercentage;
        var gross = amount * 100 / (100 - level1TaxPercentage);
        //only lvl1 tax is applicable
        if (gross <= level1Threshold)
            return gross;

        return (100 * amount + 100 * tax1 - level1Threshold * level2TaxPercentage) / (100 - level2TaxPercentage);
    }
}