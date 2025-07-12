using System.Collections.Generic;
using System.Linq;


public class VendingRepository : IVendingRepository
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

        CoinStock = new()
        {
            { 1000, 0 }, { 500, 20 }, { 100, 30 }, { 50, 50 }, { 25, 25 }
        };
    }

    public List<Drink> GetDrinks()
    {
        return Drinks;
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
            CoinStock[money.MoneyType] += money.Quantity;
        }

        int changeToReturn = totalInserted - totalCost;

        var changeResult = CalculateChange(changeToReturn);

        if (!changeResult.Success)
        {
            foreach (var money in request.MoneyInserted)
                CoinStock[money.MoneyType] -= money.Quantity;

            return changeResult;
        }

        foreach (var (drink, quantity) in drinkUpdates)
            drink.Quantity -= quantity;

        return new()
        {
            Success = true,
            Message = $"Compra exitosa!! Su vuelto es de {changeToReturn} colones",
            ChangeBreakdown = changeResult.ChangeBreakdown
        };
    }

    private ChangeResult CalculateChange(int amount)
    {
        var moneyTypes = CoinStock.Keys.OrderByDescending(k => k).ToList();
        var result = new Dictionary<int, int>();
        int remaining = amount;

        foreach (var money in moneyTypes)
        {
            int needed = remaining / money;
            int available = CoinStock[money];
            int used = Math.Min(needed, available);

            if (used > 0)
            {
                result[money] = used;
                remaining -= money * used;
            }
        }

        if (remaining == 0)
        {
            foreach (var v in result)
                CoinStock[v.Key] -= v.Value;

            return new ChangeResult { Success = true, ChangeBreakdown = result };
        }

        return new ChangeResult { Success = false, Message = "No hay cambio suficiente" };
    }
}