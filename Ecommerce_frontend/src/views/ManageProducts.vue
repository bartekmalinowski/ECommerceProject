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
    <div class="product-list">
      <h2>Lista Produktów</h2>
      <div v-if="loading">Ładowanie...</div>
      <div v-else-if="products.length === 0">Brak produktów do wyświetlenia.</div>
      <ul v-else>
        <li v-for="product in products" :key="product.id">
          <div class="product-info">
            <strong>{{ product.name }}</strong>
            <span>{{ product.price.toFixed(2) }} PLN</span>
            <p>{{ product.description }}</p>
          </div>
          <div class="product-actions">
            <button @click="editProduct(product)">Edytuj</button>
            <button @click="removeProduct(product.id)" class="delete-btn">Usuń</button>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
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
      const productToSend = { 
        name: currentProduct.value.name,
        description: currentProduct.value.description,
        price: currentProduct.value.price
      };
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

function editProduct(product) {
  isEditing.value = true;
  currentProduct.value = { ...product };
}

function resetForm() {
  isEditing.value = false;
  currentProduct.value = { ...defaultProductState };
}

function cancelEdit() {
  resetForm();
}

onMounted(fetchProducts);
</script>

<style scoped>
.container { max-width: 800px; margin: 2rem auto; padding: 0 1rem; }
.form-card, .product-list li { background: #fff; border: 1px solid #ddd; padding: 20px; border-radius: 8px; margin-bottom: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.05); }
.form-card { background: #f9f9f9; }
h1, h2, strong { color: #333; }
form { display: flex; flex-direction: column; gap: 15px; }
input, textarea { padding: 10px; border-radius: 4px; border: 1px solid #ccc; font-size: 1rem; }
.form-actions { display: flex; gap: 10px; }
button { padding: 10px 15px; border: none; border-radius: 4px; cursor: pointer; background-color: #007bff; color: white; transition: background-color 0.2s; }
button:hover { background-color: #0056b3; }
.cancel-btn { background-color: #6c757d; }
.delete-btn { background-color: #dc3545; }
.product-list ul { list-style: none; padding: 0; display: flex; flex-direction: column; gap: 15px; }
.product-list li { display: flex; justify-content: space-between; align-items: center; margin-bottom: 0; }
.product-info { flex-grow: 1; }
.product-info span { margin-left: 10px; font-weight: bold; color: #28a745; }
.product-info p { margin: 5px 0 0 0; color: #666; font-size: 0.9rem; }
.product-actions { display: flex; gap: 10px; flex-shrink: 0; margin-left: 20px; }
</style>