public class VendingService
{
    public List<Drink> Drinks { get; set; }
    public Dictionary<int, int> CoinStock { get; set; } 

    public VendingService()
    {
        Drinks = new()
        {
            new Drink { Name = "Coca Cola", Price = 800, Quantity = 10 },
            new Drink { Name = "Pepsi", Price = 750, Quantity = 8 },
            new Drink { Name = "Fanta", Price = 950, Quantity = 10 },
            new Drink { Name = "Sprite", Price = 975, Quantity = 15 }
        };
         
        // Aquí es donde se tienen las monedas para dar de vuelto
        CoinStock = new()
        {
            { 1000, 0 }, { 500, 20 }, { 100, 30 }, { 50, 50 }, { 25, 25 }
        };
    }
}
