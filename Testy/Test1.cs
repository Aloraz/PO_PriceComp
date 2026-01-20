using projektPO.projektPO;
using projektPO;

namespace Testy
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Test_OnlineStore_ObliczanieCenyCalkowitej()
        {
            // Arrange
            var sklep = new OnlineStore("Allegro", 15.00m);
            var produkt = new Product("Kawa", 0.5m, "kg");

            // Act
            var oferta = new Offer(produkt, sklep, 40.00m);

            // Assert
            Assert.AreEqual(55.00m, oferta.TotalPrice);
        }
        [TestMethod]
        public void Test_Product_CenaJednostkowaPowinnaBycPoprawna()
        {
            // Arrange
            var produkt = new Product("Cukier", 2.0m, "kg");
            var sklep = new LocalStore("Lidl");

            // Act
            var oferta = new Offer(produkt, sklep, 10.00m);

            // Assert
            Assert.AreEqual(5.00m, oferta.UnitPrice);
        }
        [TestMethod]
        public void Test_Offer_WyjatekPrzyUjemnej()
        {
            // Arrange
            var sklep = new LocalStore("Biedronka");
            var produkt = new Product("Chleb");

            // Act & Assert
            Assert.ThrowsException<InvalidPriceException>(() =>
                new Offer(produkt, sklep, -5.00m)
            );
        }
        [TestMethod]
        public void Test_Product_Equals()
        {
            // Arrange
            var p1 = new Product("Mleko", 1.0m, "l");
            var p2 = new Product("mleko", 1.0m, "l"); // mała litera, ale ta sama nazwa

            // Act
            bool saRowne = p1.Equals(p2);

            // Assert
            Assert.IsTrue(saRowne, "Błąd: IEquatable nie rozpoznał produktów mimo tej samej nazwy i wagi.");
        }
        [TestMethod]
        public void Test_Product_Clone_PowinienStworzycKopieObiektu()
        {
            // Arrange
            var oryginal = new Product("Cukier", 1.0m, "kg");

            // Act
            var klon = (Product)oryginal.Clone();

            // Assert
            Assert.AreNotSame(oryginal, klon);
            Assert.AreEqual(oryginal.Name, klon.Name);
        }
    }
}
