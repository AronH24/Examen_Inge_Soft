public class DrinkRequest
{
    public string DrinkName { get; set; }
    public int Quantity { get; set; }
}

public class PurchaseRequest
{
    public List<DrinkRequest> Drinks { get; set; }
    public List<Money> MoneyInserted { get; set; }
}
