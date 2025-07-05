<template>
  <div class="container">
    <h1>Zarządzanie Zamówieniami</h1>
    <div class="form-card">
      <h2>Stwórz Nowe Zamówienie</h2>
      <form @submit.prevent="handleCreateOrder">
        <input v-model="newOrder.customerName" placeholder="Imię i nazwisko klienta" required />
        <div class="order-item-adder">
          <select v-model="selectedProductId"><option disabled value="">Wybierz produkt</option><option v-for="product in products" :key="product.id" :value="product.id">{{ product.name }} ({{ product.price.toFixed(2) }} PLN)</option></select>
          <input v-model.number="itemQuantity" type="number" min="1" placeholder="Ilość" />
          <button @click="addProductToOrder" type="button">Dodaj</button>
        </div>
        <div v-if="newOrder.items.length > 0" class="new-order-items">
          <h4>Produkty w zamówieniu:</h4>
          <ul><li v-for="(item, index) in newOrder.items" :key="index">
              {{ getProductName(item.productId) }} - Ilość: {{ item.quantity }}
              <button @click="removeItemFromOrder(index)" type="button" class="remove-item-btn">✕</button>
          </li></ul>
        </div>
        <button type="submit" :disabled="!newOrder.customerName || newOrder.items.length === 0">Złóż zamówienie</button>
      </form>
    </div>

    <div class="list-container">
      <h2>Lista Zamówień</h2>
      <div v-if="loading">Ładowanie...</div>
      <div v-else-if="orders.length === 0">Brak zamówień.</div>
      <div v-else class="item-list">
        <div v-for="order in orders" :key="order.id" class="order-card">
          <div v-if="editingOrderId === order.id" class="edit-mode">
            <h3 class="edit-title">Edycja zamówienia #{{ order.id }}</h3>
            <input v-model="editingCustomerName" placeholder="Imię i nazwisko klienta" class="customer-name-input" />
            <h4>Pozycje w zamówieniu:</h4>
            <ul class="editing-items-list"><li v-for="(item, index) in editingOrderItems" :key="index">
                <span>{{ getProductName(item.productId) }}</span>
                <input v-model.number="item.quantity" type="number" min="1" class="quantity-input" />
                <button @click="removeItemFromEditList(index)" class="remove-item-btn">✕</button>
            </li><li v-if="editingOrderItems.length === 0">Brak produktów. Dodaj poniżej.</li></ul>
            <div class="order-item-adder">
              <select v-model="editSelectedProductId"><option disabled value="">Dodaj nowy produkt...</option><option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option></select>
              <input v-model.number="editItemQuantity" type="number" min="1" /><button @click="addItemToEditList" type="button">Dodaj</button>
            </div>
            <div class="form-actions"><button @click="handleUpdateOrder(order.id)" class="save-btn">Zapisz zmiany</button><button @click="cancelEdit" class="cancel-btn">Anuluj</button></div>
          </div>
          <div v-else>
            <div class="order-header">
              <h3 class="item-title">Zamówienie #{{ order.id }} - {{ order.customerName }}</h3>
              <strong class="item-price">{{ order.totalAmount.toFixed(2) }} PLN</strong>
            </div>
            <p class="order-date">Data: {{ new Date(order.orderDate).toLocaleString() }}</p>
            <ul><li v-for="item in order.items" :key="`${order.id}-${item.productId}`" class="order-item-detail">
                <span>{{ item.productName }}</span>
                <span>{{ item.quantity }} x {{ item.price.toFixed(2) }} PLN</span>
            </li></ul>
            <div class="item-actions"><button @click="startEdit(order)">Edytuj</button><button @click="removeOrder(order.id)" class="delete-btn">Usuń</button></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// Cała sekcja <script> pozostaje bez zmian
import { ref, onMounted } from 'vue';
import { getOrders, createOrder, deleteOrder, updateOrder, getProducts } from '../services/apiService';

const orders = ref([]); const products = ref([]); const loading = ref(true);
const newOrder = ref({ customerName: '', items: [] }); const selectedProductId = ref(''); const itemQuantity = ref(1);
const editingOrderId = ref(null); const editingCustomerName = ref(''); const editingOrderItems = ref([]); const editSelectedProductId = ref(''); const editItemQuantity = ref(1);

async function fetchData() {
  try {
    loading.value = true;
    const [ordersResponse, productsResponse] = await Promise.all([getOrders(), getProducts()]);
    orders.value = ordersResponse.data.sort((a, b) => b.id - a.id);
    products.value = productsResponse.data;
  } catch (error) { console.error("Błąd podczas pobierania danych:", error); } 
  finally { loading.value = false; }
}
function addProductToOrder() {
  if (!selectedProductId.value || itemQuantity.value < 1) return;
  const existingItem = newOrder.value.items.find(item => item.productId === selectedProductId.value);
  if (existingItem) { existingItem.quantity += itemQuantity.value; } 
  else { newOrder.value.items.push({ productId: selectedProductId.value, quantity: itemQuantity.value }); }
  selectedProductId.value = ''; itemQuantity.value = 1;
}
function removeItemFromOrder(index) { newOrder.value.items.splice(index, 1); }
async function handleCreateOrder() {
  try {
    await createOrder(newOrder.value);
    newOrder.value = { customerName: '', items: [] };
    await fetchData();
  } catch (error) { console.error("Błąd podczas tworzenia zamówienia:", error); alert('Wystąpił błąd: ' + (error.response?.data || error.message)); }
}
function startEdit(order) {
  editingOrderId.value = order.id; editingCustomerName.value = order.customerName;
  editingOrderItems.value = JSON.parse(JSON.stringify(order.items.map(item => ({ productId: item.productId, quantity: item.quantity }))));
}
function cancelEdit() {
  editingOrderId.value = null; editingCustomerName.value = ''; editingOrderItems.value = [];
  editSelectedProductId.value = ''; editItemQuantity.value = 1;
}
function addItemToEditList() {
    if (!editSelectedProductId.value || editItemQuantity.value < 1) { alert("Wybierz produkt i podaj ilość."); return; }
    const existingItem = editingOrderItems.value.find(item => item.productId === editSelectedProductId.value);
    if(existingItem) { existingItem.quantity += editItemQuantity.value; } 
    else { editingOrderItems.value.push({ productId: editSelectedProductId.value, quantity: editItemQuantity.value }); }
    editSelectedProductId.value = ''; editItemQuantity.value = 1;
}
function removeItemFromEditList(index) { editingOrderItems.value.splice(index, 1); }
async function handleUpdateOrder(orderId) {
  if (!editingCustomerName.value.trim() || editingOrderItems.value.length === 0) { alert('Nazwa klienta i co najmniej jeden produkt są wymagane.'); return; }
  const payload = { customerName: editingCustomerName.value, items: editingOrderItems.value };
  try {
    await updateOrder(orderId, payload);
    cancelEdit(); await fetchData();
  } catch (error) { console.error("Błąd podczas aktualizacji zamówienia:", error); alert('Nie udało się zaktualizować zamówienia: ' + (error.response?.data || error.message)); }
}
async function removeOrder(id) {
  if (confirm('Czy na pewno chcesz usunąć to zamówienie?')) {
    try { await deleteOrder(id); await fetchData(); } 
    catch (error) { console.error("Błąd podczas usuwania zamówienia:", error); }
  }
}
function getProductName(productId) {
    const product = products.value.find(p => p.id === productId);
    return product ? product.name : 'Nieznany produkt';
}
onMounted(fetchData);
</script>

<style scoped>
/* Style przeniesione do globalnego main.css, ale dodajemy specyficzne dla tego komponentu */
form { display: flex; flex-direction: column; gap: 1rem; }
.form-actions { display: flex; gap: 1rem; margin-top: 1rem; }
.order-item-adder { display: grid; grid-template-columns: 3fr 1fr auto; gap: 1rem; align-items: center; margin: 1rem 0; }
.new-order-items { margin-top: 1rem; background: #fdfdff; padding: 1rem; border-radius: 8px; border: 1px solid #eee; }
.new-order-items ul, .editing-items-list { list-style: none; padding: 0; }
.new-order-items li, .editing-items-list li { display: flex; justify-content: space-between; align-items: center; padding: 0.5rem 0; border-bottom: 1px solid #eee; }
.remove-item-btn { background-image: none; background-color: #f8f9fa; color: var(--danger-color); padding: 0.1rem 0.5rem; font-size: 1.2rem; font-weight: bold; line-height: 1; }
.item-list { list-style: none; padding: 0; display: flex; flex-direction: column; gap: 1.5rem; }
.order-card { padding: 1.5rem; background: var(--card-background); border-radius: var(--border-radius); box-shadow: var(--box-shadow); }
.order-header { display: flex; justify-content: space-between; align-items: flex-start; }
.item-title { font-size: 1.2rem; font-weight: 600; color: var(--text-color); margin: 0; }
.item-price { font-weight: 700; color: var(--primary-color); font-size: 1.2rem; }
.order-date { font-size: 0.85rem; color: #999; margin: 0.25rem 0 1rem 0; }
.order-card ul { list-style: none; padding: 0.5rem 0; margin: 0; border-top: 1px solid #f0f0f0; border-bottom: 1px solid #f0f0f0; }
.order-item-detail { display: flex; justify-content: space-between; font-size: 0.95rem; padding: 0.5rem 0; }
.item-actions { display: flex; gap: 0.75rem; margin-top: 1rem; }
.edit-mode .edit-title { border: none; font-size: 1.2rem; }
.edit-mode .customer-name-input { margin-bottom: 1rem; }
.edit-mode .quantity-input { width: 60px; text-align: center; }
</style>