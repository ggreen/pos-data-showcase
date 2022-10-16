namespace pos_consumer.Domain
{
    public record Product(
        string id,
        string name ,
        double price,
        string details,
        string ingredients,
        string directions,
        string warnings,
        string quantityAmount,
        Nutrition nutrition);

    public record Nutrition(
        int totalFatAmount,
        int cholesterol,
        int sodium,
        int totalCarbohydrate,
        int sugars,
        int protein);

}