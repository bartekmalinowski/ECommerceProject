import { createRouter, createWebHistory } from 'vue-router'
// Usunęliśmy import HomeView

// Importujemy tylko te widoki, które istnieją
import ManageProducts from '../views/ManageProducts.vue'
import ManageOrders from '../views/ManageOrders.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      // Gdy użytkownik wejdzie na stronę główną ('/'),
      // zostanie automatycznie przekierowany na '/products'.
      path: '/',
      redirect: '/products'
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