public interface IVendingRepository
{
    List<Drink> GetDrinks();
    ChangeResult Purchase(PurchaseRequest request);
}