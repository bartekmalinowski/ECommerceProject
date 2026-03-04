# E-Commerce Project - .NET API + Vue.js Frontend

Pełnostakowa aplikacja do zarządzania produktami i zamówieniami z automatycznym wdrażaniem na Azure.

---

## Live Demo - Azure (Kliknij i testuj!)

| Komponent | Link |
|-----------|------|
| **Frontend** | [https://delightful-stone-0b3f82703.1.azurestaticapps.net](https://delightful-stone-0b3f82703.1.azurestaticapps.net) |
| **Swagger API** | [https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger](https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/swagger) |
| **API Endpoint** | [https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/api](https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/api) |

---

## Co możesz robić?

- **Frontend** - Pełna aplikacja do zarządzania produktami i zamówieniami
- **Swagger** - Testuj wszystkie API endpointy interaktywnie
- **REST API** - GET, POST, PUT, DELETE dla produktów i zamówień

---

## Architektura

```
Vue.js Frontend (Azure Static Web Apps)
         ↓ HTTP/HTTPS
.NET 8 Web API (Azure App Service)
         ↓
In-Memory Database
```

---

## Technologie

- **Backend:** .NET 8, ASP.NET Core, Entity Framework Core
- **Frontend:** Vue.js 3, Vite, Axios, Vue Router
- **Database:** In-Memory Database (Entity Framework)
- **Cloud:** Microsoft Azure (App Service, Static Web Apps)
- **CI/CD:** GitHub Actions

---

## Jak uruchomić lokalnie?

### Backend (API)
```bash
cd ECommerceProject
dotnet restore
dotnet run
# Swagger: https://localhost:5106/swagger
```

### Frontend
```bash
cd Ecommerce_frontend
npm install
npm run dev
# App: http://localhost:5173
```

### Zmiana na Azure API (z lokalnego frontendu)
Edytuj `Ecommerce_frontend/src/services/apiService.js`:
```javascript
baseURL: 'https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/api'
```

---

## API Endpointy

### Produkty
- `GET /api/products` - Wszystkie produkty
- `POST /api/products` - Nowy produkt
- `PUT /api/products/{id}` - Edytuj produkt
- `DELETE /api/products/{id}` - Usuń produkt

### Zamówienia
- `GET /api/orders` - Wszystkie zamówienia
- `GET /api/orders/{id}` - Jedno zamówienie
- `POST /api/orders` - Nowe zamówienie
- `PUT /api/orders/{id}` - Edytuj zamówienie
- `DELETE /api/orders/{id}` - Usuń zamówienie

---

## Usługi Azure

| Usługa | Opis |
|--------|------|
| **Azure App Service** | Hosting backendu (.NET API) |
| **Azure Static Web Apps** | Hosting frontendu (Vue.js) |
| **GitHub Actions** | Automatyczne CI/CD |

---

## Jak to działa na Azure?

1. **Push do `main` na GitHub**
2. **GitHub Actions uruchamia workflow**
   - Buduje .NET API
   - Publikuje kod
   - Wdraża na App Service
3. **Frontend buduje się automatycznie**
   - Wdrażane na Static Web Apps
4. **Aplikacja dostępna publicznie** w ~2-3 minuty

---

## Troubleshooting

| Problem | Rozwiązanie |
|---------|------------|
| API timeout | Czekaj 1 minutę - App Service może się wybudowywać |
| Frontend NOT found | Sprawdź czy Static Web App secret jest ustawiony |
| CORS error | Frontend nie ma dostępu do API - sprawdź apiService.js |

---

## Główne Funkcjonalności

✅ Zarządzanie produktami (CRUD)
✅ Zarządzanie zamówieniami (CRUD)
✅ Relacja wiele-do-wielu (1 zamówienie → wiele produktów)
✅ Edycja zamówień (zmiana produktów, ilości)
✅ Asynchroniczne operacje
✅ Automatyczne wdrażanie na Azure

---

