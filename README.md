---

## Automatyzacja Wdrożenia (CI/CD)

Projekt wykorzystuje **GitHub Actions** do automatyzacji procesu budowania i wdrażania (CI/CD - Continuous Integration / Continuous Deployment).

### Przebieg Procesu

Workflow jest zdefiniowany w pliku `.github/workflows/dotnet-deploy.yml` i uruchamia się automatycznie po każdym `push` do gałęzi `main`. Składa się on z dwóch głównych etapów (zadań):

#### 1. Etap Budowania (`build`)
To zadanie jest odpowiedzialne za przygotowanie aplikacji do wdrożenia.
1.  **Pobranie Kodu:** Klonowanie repozytorium na maszynę wirtualną (runner).
2.  **Konfiguracja Środowiska:** Instalacja odpowiedniej wersji .NET SDK.
3.  **Budowanie Aplikacji:** Kompilacja kodu źródłowego API w konfiguracji `Release`.
4.  **Publikacja:** Utworzenie gotowej do wdrożenia paczki z plikami aplikacji.
5.  **Archiwizacja Artefaktu:** Wynik budowania jest zapisywany jako artefakt w GitHub Actions, aby mógł być wykorzystany w kolejnym etapie.

#### 2. Etap Wdrożenia (`deploy`)
To zadanie jest uruchamiane tylko po pomyślnym zakończeniu etapu `build`.
1.  **Pobranie Artefaktu:** Pobranie paczki z aplikacją, stworzonej w poprzednim zadaniu.
2.  **Uwierzytelnienie w Azure:** Logowanie do platformy Microsoft Azure przy użyciu bezpiecznie przechowywanego profilu publikacji.
3.  **Wdrożenie w Azure App Service:** Wysłanie plików aplikacji na serwer Azure, gdzie zostają one automatycznie uruchomione.

### Środowisko Docelowe
-   **Platforma:** Microsoft Azure
-   **Usługa:** Azure App Service
-   **Adres API:** [https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger](https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger)

Dzięki tej automatyzacji, każda zmiana wprowadzona do głównej gałęzi projektu jest natychmiast testowana, budowana i wdrażana, co zapewnia spójność i szybkość dostarczania nowych wersji aplikacji.