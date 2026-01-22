## DiagramCRC

| **Product** | |
| :--- | :--- |
| Przechowuje nazwę, ilość i jednostkę produktu. | - |
| Sprawdza poprawność danych  | - |
| Klonowanie obiektu (ICloneable). | - |

| **Store** | |
| :--- | :--- |
| Definiuje wspólny interfejs dla wszystkich typów sklepów. | - |
| Udostępnia metodę obliczania dodatkowych kosztów (dostawy). | - |

| **Offer** | |
| :--- | :--- |
| Powiązuje produkt z konkretnym sklepem i ceną. | **Product, Store** |
| Oblicza cenę jednostkową (na podstawie ilości z produktu). | **Product** |
| Oblicza całkowity koszt (cena + koszty sklepu). | **Store** |

| **ShoppingLogic** | |
| :--- | :--- |
| Przeszukuje dostępne oferty według zadanego kryterium. | **Offer** |
| Oblicza optymalny koszyk zakupowy dla listy produktów. | **Offer, Store** |
| Porównuje oferty pod kątem ceny jednostkowej. | **Offer** |

| **DataManager** | |
| :--- | :--- |
| Zapisuje listę ofert do pliku JSON. | **Offer** |
| Wczytuje i deserializuje dane z dysku. | **Offer** |
| Obsługuje polimorfizm typów sklepów podczas zapisu. | **Store** |

| **DataSeeder** | |
| :--- | :--- |
| Generuje testowe zestawy danych (sklepy, produkty, oferty). | **Offer, Product, LocalStore, OnlineStore** |
| Inicjalizuje bazę danych przy pierwszym uruchomieniu. | **DataManager** |