import { createRouter, createWebHistory } from 'vue-router'
// NOWOŚĆ: Importujemy nasz nowy widok strony głównej
import HomeView from '../views/HomeView.vue'
import ManageProducts from '../views/ManageProducts.vue'
import ManageOrders from '../views/ManageOrders.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      // ZMIANA: Zamiast przekierowania, ustawiamy komponent dla ścieżki głównej
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/products',
      name: 'manage-products',
      component: ManageProducts
    },
    {
      path: '/orders',
      name: 'manage-orders',
      component: ManageOrders
    }
  ]
})

export default router