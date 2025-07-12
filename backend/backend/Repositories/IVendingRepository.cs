using backend.Models;
public interface IVendingRepository
{
    List<Drink> GetDrinks();
    ChangeResult Purchase(PurchaseRequest request);
}