public class VendingRepository
{
    public List<Drink> Drinks { get; set; }
    public Dictionary<int, int> CoinStock { get; set; }

    public VendingRepository()
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

        public ChangeResult Purchase(PurchaseRequest request)
        {
            int totalCost = 0;
            var drinkUpdates = new List<(Drink drink, int quantity)>();
             
            foreach (var item in request.Drinks)
            {
                var drink = Drinks.FirstOrDefault(drink => drink.Name == item.DrinkName);
                if (drink == null)
                    return new() { Success = false, Message = $"El refresco '{item.DrinkName}' no pudo ser encontrado" };

                if (item.Quantity > drink.Quantity)
                    return new() { Success = false, Message = $"No hay suficientes latas de '{item.DrinkName}'" };

                totalCost += drink.Price * item.Quantity;
                drinkUpdates.Add((drink, item.Quantity));
            }

            int totalInserted = request.MoneyInserted.Sum(money => money.MoneyType * money.Quantity);

            if (totalInserted < totalCost)
                return new()
                {
                    Success = false,
                    Message = "El dinero insertado es insuficiente para realizar la compra"
                };
         
            foreach (var money in request.MoneyInserted)
            {
              // Las monedas que ingresa la persona se suman al inventario 
              CoinStock[money.MoneyType] += money.Quantity;
            }

        
            int changeToReturn = totalInserted - totalCost;
            
            // TODO: Agregar luego la lógica para dar el vuelto

            // Aquí se disminuye el inventario si todo sale bien
            foreach (var (drink, quantity) in drinkUpdates)
                drink.Quantity -= quantity;
   
            var changeResult = new ChangeResult();
            changeResult.ChangeBreakdown = new Dictionary<int, int>();
            
            return new()
            {
                Success = true,
                Message = $"Compra exitosa!! Su vuelto es de {changeToReturn} colones",
                ChangeBreakdown = changeResult.ChangeBreakdown
            };
        }
    
}
