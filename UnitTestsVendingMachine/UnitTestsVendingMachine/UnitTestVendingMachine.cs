using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using backend.Models;

namespace UnitTestsVendingMachine
{
    public class VendingRepositoryTests
    {
        private VendingRepository _vendingRepository;

        [SetUp]
        public void Setup()
        {
            _vendingRepository = new VendingRepository();
        }

        [Test]
        public void GetDrinks_ShouldReturnAllDrinks()
        {
            // Act
            var drinks = _vendingRepository.GetDrinks();

            // Assert
            Assert.IsNotNull(drinks);
            Assert.AreEqual(4, drinks.Count);
            Assert.IsTrue(drinks.Any(d => d.Name == "Coca Cola"));
            Assert.IsTrue(drinks.Any(d => d.Name == "Pepsi"));
            Assert.IsTrue(drinks.Any(d => d.Name == "Fanta"));
            Assert.IsTrue(drinks.Any(d => d.Name == "Sprite"));
        }

        [Test]
        public void Purchase_ValidPurchaseWithExactMoney_ShouldReturnSuccess()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Coca Cola", Quantity = 1 }
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 500, Quantity = 1 },
                    new Money { MoneyType = 100, Quantity = 3 }
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Compra exitosa!! Su vuelto es de 0 colones", result.Message);
            
            var cocaCola = _vendingRepository.Drinks.First(d => d.Name == "Coca Cola");
            Assert.AreEqual(9, cocaCola.Quantity);
        }

        [Test]
        public void Purchase_ValidPurchaseWithChange_ShouldReturnSuccessWithCorrectChange()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Pepsi", Quantity = 1 } 
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 1000, Quantity = 1 } 
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Compra exitosa!! Su vuelto es de 250 colones", result.Message);
            Assert.IsNotNull(result.ChangeBreakdown);
            
      
            Assert.AreEqual(2, result.ChangeBreakdown[100]);
            Assert.AreEqual(1, result.ChangeBreakdown[50]);
        }

        [Test]
        public void Purchase_DrinkNotFound_ShouldReturnFailure()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Heineken", Quantity = 1 }
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 1000, Quantity = 1 }
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("El refresco 'Heineken' no pudo ser encontrado", result.Message);
        }

        [Test]
        public void Purchase_InsufficientStock_ShouldReturnFailure()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Coca Cola", Quantity = 15 } 
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 1000, Quantity = 20 }
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No hay suficientes latas de 'Coca Cola'", result.Message);
        }

        [Test]
        public void Purchase_InsufficientMoney_ShouldReturnFailure()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Coca Cola", Quantity = 1 } 
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 500, Quantity = 1 } 
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("El dinero insertado es insuficiente para realizar la compra", result.Message);
        }

        [Test]
        public void Purchase_MultipleDrinks_ShouldCalculateCorrectTotal()
        {
            // Arrange
            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Coca Cola", Quantity = 2 }, 
                    new DrinkRequest { DrinkName = "Pepsi", Quantity = 1 }     
                    // Total: 2350
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 1000, Quantity = 2 }, 
                    new Money { MoneyType = 500, Quantity = 1 }   
                  
                }
            };

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Compra exitosa!! Su vuelto es de 150 colones", result.Message);
            
          
            var cocaCola = _vendingRepository.Drinks.First(d => d.Name == "Coca Cola");
            var pepsi = _vendingRepository.Drinks.First(d => d.Name == "Pepsi");
            Assert.AreEqual(8, cocaCola.Quantity);
            Assert.AreEqual(7, pepsi.Quantity);    
        }

        [Test]
        public void Purchase_InsufficientChangeStock_ShouldReturnFailureAndRevertCoins()
        {
            // Arrange
         
            _vendingRepository.CoinStock[100] = 0;
            _vendingRepository.CoinStock[50] = 0;
            _vendingRepository.CoinStock[25] = 0;

            var request = new PurchaseRequest
            {
                Drinks = new List<DrinkRequest>
                {
                    new DrinkRequest { DrinkName = "Coca Cola", Quantity = 1 } 
                },
                MoneyInserted = new List<Money>
                {
                    new Money { MoneyType = 1000, Quantity = 1 } 
                }
            };

            var originalCoinStock = _vendingRepository.CoinStock[1000];

            // Act
            var result = _vendingRepository.Purchase(request);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("No hay cambio suficiente", result.Message);
            
         
            Assert.AreEqual(originalCoinStock, _vendingRepository.CoinStock[1000]);
        }

    }
}