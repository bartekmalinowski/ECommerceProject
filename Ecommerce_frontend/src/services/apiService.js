import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://bartek-ecommerceapi-d0h5dcgfdzephvgt.polandcentral-01.azurewebsites.net/api', // Azure API
    headers: {
        'Content-Type': 'application/json'
    }
});

// --- Product Methods ---
export const getProducts = () => apiClient.get('/products');
export const createProduct = (product) => apiClient.post('/products', product);
export const updateProduct = (id, product) => apiClient.put(`/products/${id}`, product);
export const deleteProduct = (id) => apiClient.delete(`/products/${id}`);

// --- Order Methods ---
// NOWOŚĆ: Dodane metody dla zamówień
export const getOrders = () => apiClient.get('/orders');
export const createOrder = (order) => apiClient.post('/orders', order);
export const deleteOrder = (id) => apiClient.delete(`/orders/${id}`);
export const updateOrder = (id, orderData) => apiClient.put(`/orders/${id}`, orderData);