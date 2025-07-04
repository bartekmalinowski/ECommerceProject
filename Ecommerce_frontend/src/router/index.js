import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ManageProducts from '../views/ManageProducts.vue'
import ManageOrders from '../views/ManageOrders.vue' // NOWOŚĆ: Import widoku zamówień

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/products',
      name: 'manage-products',
      component: ManageProducts
    },
    { // NOWOŚĆ: Dodanie ścieżki dla zamówień
      path: '/orders',
      name: 'manage-orders',
      component: ManageOrders
    }
  ]
})

export default router