# 🛒 Asystent Zakupowy (PriceComp)

**Asystent Zakupowy** to inteligentna aplikacja w technologii .NET 8.0, pomagająca optymalizować koszty codziennych zakupów. System analizuje oferty z różnych sklepów (stacjonarnych i online), porównuje ceny jednostkowe (Smart Unit Pricing) i wskazuje, gdzie najtaniej zrobisz całe zakupy.

---

## 🚀 Jak uruchomić projekt?

### Wymagania
* System Windows (dla wersji WPF)
* .NET 8.0 SDK
* Visual Studio 2022

### Uruchomienie
Projekt składa się z dwóch interfejsów działających na tej samej logice:
1. **Aplikacja Konsolowa:** Ustaw projekt `projektPO` jako startowy.
2. **Aplikacja Okienkowa (GUI):** Ustaw projekt `PriceComp.GUI` jako startowy.

---

## 🖥️ Instrukcja Obsługi: Wersja Konsolowa

Po uruchomieniu zobaczysz menu główne. Nawigacja odbywa się poprzez klawisze numeryczne `1-6`.

### 1. Przegląd Bazy Ofert (Klawisz `1`)
Wyświetla listę wszystkich dostępnych produktów w bazie wraz z ich cenami w konkretnych sklepach.

### 2. Sprawdź Okazje / Smart Unit Pricing (Klawisz `2`)
To najważniejsza funkcja dla pojedynczego produktu.
1. Wybierz opcję `2`.
2. Wpisz nazwę produktu (np. `Cola`, `Masło`).
3. **Wynik:** Otrzymasz tabelę posortowaną według **Ceny Jednostkowej** (zł/kg, zł/l).
   * *Zielony kolor* oznacza aktywną promocję.
   * Funkcja pozwala wykryć, czy "Wielka Paka" faktycznie jest tańsza niż małe opakowanie.

### 3. Kalkulator Koszyka (Klawisz `3`)
Funkcja optymalizująca całą listę zakupów.
1. Wybierz opcję `3`.
2. Wpisuj produkty po kolei, zatwierdzając `ENTER`.
3. Aby zakończyć, wciśnij `ENTER` na pustym polu.
4. **Wynik:** System wskaże ranking sklepów.
   * Dowiesz się, gdzie suma paragonu (wliczając ew. dostawę) będzie najniższa.
   * System ostrzeże Cię, jeśli w którymś sklepie brakuje produktów z Twojej listy.

### 4. Zarządzanie Bazą (Klawisze `4`, `5`, `6`)
* **Dodaj Ofertę (`5`):** Pozwala ręcznie dodać nowy produkt do bazy.
* **Zapisz (`4`):** Zapisuje aktualny stan bazy do pliku `oferty.json`.
* **Reset Bazy (`6`):** Usuwa wszelkie zmiany i przywraca fabryczne dane testowe (przydatne w razie błędów).

---

## 🎨 Instrukcja Obsługi: Wersja Graficzna (WPF)

Interfejs graficzny oferuje wygodniejszą obsługę za pomocą myszki.

### Główny Widok
1. **Lista Produktów (Lewa strona):**
   * Kliknij dwukrotnie na produkt, aby dodać go do listy zakupów.
   * Możesz też wpisać listę ręcznie w polu tekstowym (oddzielając produkty przecinkami).

2. **Obliczanie Koszyka:**
   * Po stworzeniu listy kliknij przycisk **"Oblicz Koszyk"**.
   * Poniżej pojawi się ranking sklepów.

3. **Wybór Oferty i Płatność:**
   * Kliknij na wybrany (np. najtańszy) sklep w wynikach. Ramka zmieni kolor na niebieski.
   * Przycisk **"Przejdź do płatności"** stanie się zielony i aktywny.
   * Kliknięcie otworzy okno symulacji płatności.

4. **Dodawanie Ofert (Prawy panel):**
   * Wypełnij formularz (Sklep, Nazwa, Cena, Ilość).
   * Kliknij **"Dodaj Ofertę"**.
   * Nowy produkt natychmiast pojawi się w bazie i będzie brany pod uwagę przy obliczeniach.

---

## 🔧 Rozwiązywanie problemów

**P: Program wyświetla pustą listę ofert.**
O: Użyj opcji **Reset Bazy Danych** (Klawisz `6` w konsoli) lub usuń ręcznie plik `oferty.json` z folderu projektu. System automatycznie wygeneruje nowe dane przy starcie.

**P: Moje zmiany zniknęły po ponownym uruchomieniu.**
O: Pamiętaj, aby zawsze wybierać opcję **"4. Zapisz i Wyjdź"** w konsoli lub kliknąć **"Zapisz plik"** w wersji okienkowej przed zamknięciem aplikacji.
