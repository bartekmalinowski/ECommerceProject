<template>
  <div class="container">
    <h1>Zarządzanie Produktami</h1>
    <div class="form-card">
      <h2>{{ isEditing ? 'Edytuj Produkt' : 'Dodaj Nowy Produkt' }}</h2>
      <form @submit.prevent="handleSubmit">
        <input v-model="currentProduct.name" placeholder="Nazwa produktu" required />
        <textarea v-model="currentProduct.description" placeholder="Opis produktu" rows="3"></textarea>
        <input v-model.number="currentProduct.price" type="number" step="0.01" min="0.01" placeholder="Cena" required />
        <div class="form-actions">
          <button type="submit">{{ isEditing ? 'Zapisz zmiany' : 'Dodaj produkt' }}</button>
          <button v-if="isEditing" @click="cancelEdit" type="button" class="cancel-btn">Anuluj</button>
        </div>
      </form>
    </div>

    <div class="list-container">
      <h2>Lista Produktów</h2>
      <div v-if="loading">Ładowanie...</div>
      <div v-else-if="products.length === 0">Brak produktów do wyświetlenia.</div>
      <ul v-else class="item-list">
        <li v-for="product in products" :key="product.id" class="list-item">
          <div class="item-info">
            <strong class="item-title">{{ product.name }}</strong>
            <span class="item-price">{{ product.price.toFixed(2) }} PLN</span>
            <p class="item-description">{{ product.description }}</p>
          </div>
          <div class="item-actions">
            <button @click="editProduct(product)">Edytuj</button>
            <button @click="removeProduct(product.id)" class="delete-btn">Usuń</button>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
// Cała sekcja <script> pozostaje bez zmian
import { ref, onMounted } from 'vue';
import { getProducts, createProduct, updateProduct, deleteProduct } from '../services/apiService';

const products = ref([]);
const loading = ref(true);
const isEditing = ref(false);
const defaultProductState = { id: null, name: '', description: '', price: null };
const currentProduct = ref({ ...defaultProductState });

async function fetchProducts() {
  try {
    loading.value = true;
    const response = await getProducts();
    products.value = response.data;
  } catch (error) {
    console.error("Błąd podczas pobierania produktów:", error);
    alert('Nie udało się pobrać listy produktów.');
  } finally {
    loading.value = false;
  }
}

async function handleSubmit() {
  if (!currentProduct.value.name || !currentProduct.value.price || currentProduct.value.price <= 0) {
    alert('Nazwa i cena (większa od 0) są wymagane.');
    return;
  }
  try {
    if (isEditing.value) {
      await updateProduct(currentProduct.value.id, currentProduct.value);
    } else {
      const productToSend = { name: currentProduct.value.name, description: currentProduct.value.description, price: currentProduct.value.price };
      await createProduct(productToSend);
    }
    resetForm();
    await fetchProducts();
  } catch (error) {
    console.error("Błąd podczas zapisywania produktu:", error);
    const errorMessage = error.response?.data?.title || error.response?.data || 'Wystąpił nieznany błąd.';
    alert(`Nie udało się zapisać produktu: ${errorMessage}`);
  }
}

async function removeProduct(id) {
  if (confirm('Czy na pewno chcesz usunąć ten produkt?')) {
    try {
      await deleteProduct(id);
      await fetchProducts();
    } catch (error) {
      console.error("Błąd podczas usuwania produktu:", error);
      alert('Nie udało się usunąć produktu.');
    }
  }
}

function editProduct(product) { isEditing.value = true; currentProduct.value = { ...product }; }
function resetForm() { isEditing.value = false; currentProduct.value = { ...defaultProductState }; }
function cancelEdit() { resetForm(); }

onMounted(fetchProducts);
</script>

<style scoped>
/* Style przeniesione do globalnego main.css, ale dodajemy specyficzne dla tego komponentu */
form { display: flex; flex-direction: column; gap: 1rem; }
.form-actions { display: flex; gap: 1rem; }

.item-list { list-style: none; padding: 0; display: flex; flex-direction: column; gap: 1rem; }
.list-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  background: var(--card-background);
  border-radius: var(--border-radius);
  box-shadow: var(--box-shadow);
  transition: transform 0.2s, box-shadow 0.2s;
}
.list-item:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.08);
}
.item-info { flex-grow: 1; }
.item-title { font-size: 1.2rem; font-weight: 600; color: var(--text-color); }
.item-price {
  margin-left: 1rem;
  font-weight: 600;
  color: var(--success-color);
  background-color: rgba(40, 167, 69, 0.1);
  padding: 0.2rem 0.6rem;
  border-radius: 20px;
  font-size: 0.9rem;
}
.item-description { margin: 0.5rem 0 0 0; color: var(--text-light); font-size: 0.95rem; }
.item-actions { display: flex; gap: 0.75rem; flex-shrink: 0; margin-left: 1.5rem; }
</style>