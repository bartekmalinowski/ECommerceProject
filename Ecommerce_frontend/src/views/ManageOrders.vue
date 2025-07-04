<template>
  <div class="container">
    <h1>Zarządzanie Zamówieniami</h1>
    <div class="form-card">
      <h2>Stwórz Nowe Zamówienie</h2>
      <form @submit.prevent="handleCreateOrder">
        <input v-model="newOrder.customerName" placeholder="Imię i nazwisko klienta" required />
        <div class="order-item-adder">
          <select v-model="selectedProductId">
            <option disabled value="">Wybierz produkt</option>
            <option v-for="product in products" :key="product.id" :value="product.id">
              {{ product.name }} ({{ product.price.toFixed(2) }} PLN)
            </option>
          </select>
          <input v-model.number="itemQuantity" type="number" min="1" placeholder="Ilość" />
          <button @click="addProductToOrder" type="button">Dodaj do zamówienia</button>
        </div>
        <div v-if="newOrder.items.length > 0" class="new-order-items">
          <h4>Produkty w zamówieniu:</h4>
          <ul><li v-for="(item, index) in newOrder.items" :key="index">
              {{ getProductName(item.productId) }} - Ilość: {{ item.quantity }}
              <button @click="removeItemFromOrder(index)" type="button" class="remove-item-btn">X</button>
          </li></ul>
        </div>
        <button type="submit" :disabled="!newOrder.customerName || newOrder.items.length === 0">Złóż zamówienie</button>
      </form>
    </div>
    <div class="order-list">
      <h2>Lista Zamówień</h2>
      <div v-if="loading">Ładowanie...</div>
      <div v-else-if="orders.length === 0">Brak zamówień.</div>
      <div v-else class="order-card" v-for="order in orders" :key="order.id">
        <div v-if="editingOrderId === order.id" class="edit-mode">
          <h3>Edycja zamówienia #{{ order.id }}</h3>
          <input v-model="editingCustomerName" placeholder="Imię i nazwisko klienta" class="customer-name-input" />
          <h4>Pozycje w zamówieniu:</h4>
          <ul class="editing-items-list">
            <li v-for="(item, index) in editingOrderItems" :key="index">
              <span>{{ getProductName(item.productId) }}</span>
              <input v-model.number="item.quantity" type="number" min="1" class="quantity-input" />
              <button @click="removeItemFromEditList(index)" class="remove-item-btn">X</button>
            </li>
             <li v-if="editingOrderItems.length === 0">Brak produktów. Dodaj poniżej.</li>
          </ul>
          <div class="order-item-adder">
            <select v-model="editSelectedProductId"><option disabled value="">Dodaj nowy produkt...</option><option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option></select>
            <input v-model.number="editItemQuantity" type="number" min="1" /><button @click="addItemToEditList" type="button">Dodaj</button>
          </div>
          <div class="edit-controls">
            <button @click="handleUpdateOrder(order.id)" class="save-btn">Zapisz zmiany</button>
            <button @click="cancelEdit" class="cancel-btn">Anuluj</button>
          </div>
        </div>
        <div v-else>
          <h3>Zamówienie #{{ order.id }} - {{ order.customerName }}</h3>
          <p>Data: {{ new Date(order.orderDate).toLocaleString() }}</p>
          <p><strong>Suma: {{ order.totalAmount.toFixed(2) }} PLN</strong></p>
          <ul><li v-for="item in order.items" :key="`${order.id}-${item.productId}`">
              {{ item.productName }} ({{ item.quantity }} x {{ item.price.toFixed(2) }} PLN)
          </li></ul>
          <div class="action-buttons">
              <button @click="startEdit(order)">Edytuj</button>
              <button @click="removeOrder(order.id)" class="delete-btn">Usuń zamówienie</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
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
.container { max-width: 800px; margin: 2rem auto; padding: 0 1rem; }
.form-card, .order-card { background: #fff; border: 1px solid #ddd; padding: 20px; border-radius: 8px; margin-bottom: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.05); }
.form-card { background: #f9f9f9; }
h1, h2, h3, h4 { color: #333; }
h3 { color: #0056b3; }
input, select { padding: 10px; border-radius: 4px; border: 1px solid #ccc; font-size: 1rem; }
.order-item-adder { display: grid; grid-template-columns: 3fr 1fr auto; gap: 10px; align-items: center; margin: 15px 0; }
ul { list-style: none; padding: 0; }
li { display: flex; justify-content: space-between; align-items: center; padding: 8px 0; border-bottom: 1px solid #eee; }
li:last-child { border-bottom: none; }
.remove-item-btn { background: #ffc107; color: black; padding: 2px 8px; font-size: 12px; border-radius: 50%; border: none; cursor: pointer; }
button { padding: 10px 15px; border: none; border-radius: 4px; cursor: pointer; background-color: #007bff; color: white; transition: background-color 0.2s; }
button:disabled { background-color: #ccc; cursor: not-allowed; }
.delete-btn { background-color: #dc3545; }
.save-btn { background-color: #28a745; }
.cancel-btn { background-color: #6c757d; }
.action-buttons, .edit-controls { margin-top: 15px; display: flex; gap: 10px; }
.edit-mode .customer-name-input { width: 100%; box-sizing: border-box; margin-bottom: 15px; }
.edit-mode h4 { margin-top: 20px; }
.editing-items-list { margin-bottom: 20px; }
.editing-items-list .quantity-input { width: 60px; text-align: center; }
</style>