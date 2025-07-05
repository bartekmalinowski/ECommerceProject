# E-Commerce Project - .NET API + Vue.js Frontend

Kompletny projekt full-stack aplikacji do zarządzania produktami i zamówieniami. Składa się z backendowego REST API napisanego w .NET 8 oraz interfejsu użytkownika stworzonego w Vue.js 3. Całość jest w pełni zautomatyzowana i wdrożona w chmurze Microsoft Azure z wykorzystaniem potoków CI/CD w GitHub Actions.

## Linki do działającej aplikacji

*   **Aplikacja Frontendowa:** [https://bartek-ecommerce-frontend.azurestaticapps.net](https://bartek-ecommerce-frontend.azurestaticapps.net)
*   **Dokumentacja API (Swagger):** [https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger](https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger)

*(Uwaga: adres URL aplikacji frontendowej jest przykładowy, należy go zaktualizować na podstawie danych z portalu Azure.)*

## Główne Funkcjonalności

-   **Zarządzanie Produktami (CRUD):** Pełna obsługa tworzenia, odczytu, aktualizacji i usuwania produktów.
-   **Zarządzanie Zamówieniami (CRUD):** Tworzenie nowych zamówień z listą wybranych produktów, przeglądanie historii zamówień oraz ich usuwanie.
-   **Pełna Edycja Zamówień:** Możliwość zmiany danych klienta, a także dynamicznego dodawania/usuwania produktów i modyfikacji ich ilości w istniejącym zamówieniu.
-   **Relacja wiele-do-wielu:** Prawidłowo zaimplementowana relacja między zamówieniami a produktami z wykorzystaniem tabeli łączącej `OrderItem`.
-   **Wdrożenie w Chmurze:** Aplikacja API oraz frontend są wdrożone i publicznie dostępne w Microsoft Azure.
-   **Automatyzacja CI/CD:** W pełni zautomatyzowany proces budowania i wdrażania obu części aplikacji przy użyciu GitHub Actions.

## Technologie

| Kategoria | Technologia |
|-----------|-------------|
| **Backend** | .NET 8, ASP.NET Core Web API, Entity Framework Core 8 |
| **Frontend** | Vue.js 3 (Composition API), Vite, Axios, Vue Router |
| **Baza Danych** | In-Memory Database (na potrzeby tego projektu) |
| **Chmura** | Microsoft Azure |
| **Automatyzacja** | GitHub Actions (CI/CD) |

---

## Jak uruchomić projekt lokalnie?

### Wymagania
-   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   [Node.js i npm](https://nodejs.org/en/) (rekomendowana wersja Node.js: 20.x)

### 1. Uruchomienie Backendu (API)

Otwórz terminal w folderze projektu API i wykonaj polecenia:

```bash
# Przejdź do folderu API
cd /ECommerceApi

# Uruchom aplikację
dotnet run
```
API będzie domyślnie nasłuchiwać na portach `5000` (HTTP) i `5001` (HTTPS) lub innych, wskazanych w konsoli.

### 2. Uruchomienie Frontendu

Otwórz **drugi terminal** w folderze projektu frontendowego i wykonaj polecenia:

```bash
# Przejdź do folderu frontendu
cd ECommerceApi/Ecommerce_frontend

# Zainstaluj zależności (tylko za pierwszym razem)
npm install

# Uruchom serwer deweloperski
npm run dev
```
Aplikacja frontendowa będzie dostępna pod adresem `http://localhost:5173`.

---

## Informacje o wdrożeniu w Azure

Projekt został wdrożony w chmurze z wykorzystaniem następujących usług:

#### Wykorzystane usługi Azure
1.  **Azure App Service:**
    -   **Cel:** Hostowanie i uruchamianie aplikacji backendowej (.NET Web API).
    -   **Plan:** F1 (Free Tier) na systemie Linux.
    -   **Opis:** Zapewnia skalowalne i zarządzane środowisko do uruchamiania aplikacji internetowych.

2.  **Azure Static Web Apps:**
    -   **Cel:** Hostowanie aplikacji frontendowej (Vue.js).
    -   **Plan:** Free.
    -   **Opis:** Usługa zoptymalizowana pod kątem dostarczania statycznych zasobów (HTML, CSS, JS) i integracji z nowoczesnymi frameworkami JavaScript. Oferuje darmowy certyfikat SSL i globalną dystrybucję.

#### Informacje Konfiguracyjne
-   **Połączenie Frontend-Backend:** Aplikacja frontendowa jest skonfigurowana do komunikacji z publicznym adresem URL wdrożonego API.
-   **CORS (Cross-Origin Resource Sharing):** Usługa App Service (backend) została skonfigurowana tak, aby akceptować żądania przychodzące z adresu URL aplikacji frontendowej. Jest to niezbędne do poprawnego działania aplikacji w przeglądarce.

---

## Automatyzacja Wdrożenia (CI/CD)

Projekt wykorzystuje **GitHub Actions** do automatyzacji procesu budowania i wdrażania. W repozytorium znajdują się dwa niezależne workflowy:

### 1. `dotnet-deploy.yml` (dla API)
-   **Trigger:** `push` na gałąź `main`.
-   **Etap `build`:** Buduje aplikację .NET, tworzy paczkę wdrożeniową i zapisuje ją jako artefakt.
-   **Etap `deploy`:** Pobiera artefakt i wdraża go na Azure App Service.

### 2. `azure-static-web-apps-....yml` (dla Frontendu)
-   **Trigger:** `push` na gałąź `main`.
-   **Proces:** Automatycznie generowany przez Azure, buduje aplikację Vue.js (używając wersji Node.js zdefiniowanej w `package.json`) i wdraża wynikowe pliki statyczne na Azure Static Web Apps.

Dzięki tej automatyzacji, każda zmiana wprowadzona do głównej gałęzi projektu jest natychmiast wdrażana, co zapewnia spójność i szybkość dostarczania nowych wersji aplikacji.