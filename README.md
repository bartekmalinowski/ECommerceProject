# E-Commerce Project - .NET API + Vue.js Frontend

Kompletny projekt full-stack aplikacji do zarządzania produktami i zamówieniami. Składa się z backendowego REST API napisanego w .NET 8 oraz interfejsu użytkownika stworzonego w Vue.js 3. Projekt zawiera również konfigurację do automatycznego wdrażania w chmurze Microsoft Azure z wykorzystaniem potoków CI/CD w GitHub Actions.

## Główne Funkcjonalności

-   **Zarządzanie Produktami (CRUD):** Pełna obsługa tworzenia, odczytu, aktualizacji i usuwania produktów.
-   **Zarządzanie Zamówieniami (CRUD):** Tworzenie nowych zamówień z listą wybranych produktów, przeglądanie historii zamówień oraz ich usuwanie.
-   **Pełna Edycja Zamówień:** Możliwość zmiany danych klienta, a także dynamicznego dodawania/usuwania produktów i modyfikacji ich ilości w istniejącym zamówieniu.
-   **Relacja wiele-do-wielu:** Prawidłowo zaimplementowana relacja między zamówieniami a produktami z wykorzystaniem tabeli łączącej `OrderItem`.
-   **Automatyzacja CI/CD:** Skonfigurowane potoki w GitHub Actions do automatyzacji procesu budowania i wdrażania obu części aplikacji.

## Technologie

| Kategoria | Technologia |
|-----------|-------------|
| **Backend** | .NET 8, ASP.NET Core Web API, Entity Framework Core 8 |
| **Frontend** | Vue.js 3 (Composition API), Vite, Axios, Vue Router |
| **Baza Danych** | In-Memory Database (na potrzeby tego projektu) |
| **Automatyzacja** | GitHub Actions (CI/CD) |
| **Chmura (opcjonalnie)**| Microsoft Azure (App Service, Static Web Apps) |

---

## Jak uruchomić projekt lokalnie?

### Wymagania Wstępne
-   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   [Node.js i npm](https://nodejs.org/en/) (rekomendowana wersja Node.js: 20.x)
-   [Git](https://git-scm.com/downloads)

### Kroki Uruchomienia

1.  **Sklonuj repozytorium:**
    ```bash
    git clone https://github.com/bartekmalinowski/ECommerceProject.git
    cd ECommerceProject
    ```
    *Wszystkie kolejne polecenia wykonuj z głównego folderu `ECommerceProject`.*

2.  **Uruchom Backend (API):**

    Otwórz terminal i wykonaj poniższe polecenia:

    ```bash
    # Przejdź do folderu z projektem API
    cd ECommerceApi

    # Przywróć wszystkie zależności .NET (dobra praktyka)
    dotnet restore

    # Uruchom aplikację
    dotnet run
    ```
    API będzie domyślnie nasłuchiwać na portach wskazanych w konsoli (np. 5001 dla HTTPS). Dokumentacja Swagger będzie dostępna pod adresem `https://localhost:<PORT>/swagger`.

3.  **Uruchom Frontend:**

    Otwórz **drugi, nowy terminal** i wykonaj poniższe polecenia:

    ```bash
    # Przejdź do folderu z projektem frontendu
    cd Ecommerce_frontend

    # Zainstaluj wszystkie zależności Node.js (wymagane tylko za pierwszym razem)
    npm install

    # Uruchom serwer deweloperski
    npm run dev
    ```
    Aplikacja frontendowa będzie dostępna pod adresem `http://localhost:5173`.

### Ważna informacja konfiguracyjna

Domyślnie, kod frontendu w repozytorium może być skonfigurowany do łączenia się z API wdrożonym w chmurze. Aby połączyć lokalnie uruchomiony frontend z lokalnie uruchomionym API:

1.  Otwórz plik `Ecommerce_frontend/src/services/apiService.js`.
2.  Upewnij się, że wartość `baseURL` wskazuje na adres Twojego lokalnego API:
    ```javascript
    baseURL: 'https://localhost:5106/api', // Użyj portu HTTPS z konsoli API
    ```
