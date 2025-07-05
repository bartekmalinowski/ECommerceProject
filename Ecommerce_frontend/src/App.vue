<template>
  <!-- Ten div otacza całą aplikację -->
  <div id="app-wrapper">
    <header class="app-header">
      <div class="header-container">
        <div class="logo">
          <RouterLink to="/">🚀 E-Commerce</RouterLink>
        </div>
        <nav>
          <RouterLink to="/">Home</RouterLink>
          <RouterLink to="/products">Produkty</RouterLink>
          <RouterLink to="/orders">Zamówienia</RouterLink>
        </nav>
      </div>
    </header>

    <!-- Main jest teraz na zewnątrz nagłówka i zawiera RouterView -->
    <main>
      <RouterView />
    </main>
    
    <footer class="app-footer">
      <p>© {{ new Date().getFullYear() }} E-Commerce Project by Bartek Malinowski. All rights reserved.</p>
    </footer>
  </div>
</template>

<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<style scoped>
/* Te style pozostają "scoped", aby dotyczyły tylko tego komponentu */

#app-wrapper {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.app-header {
  background-color: var(--card-background);
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  padding: 1rem 0;
  position: sticky;
  top: 0;
  z-index: 1000;
  width: 100%;
}

.header-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo a {
  font-size: 1.5rem;
  font-weight: 700;
  text-decoration: none;
  background: linear-gradient(45deg, var(--primary-color), var(--secondary-color));
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

nav {
  display: flex;
  gap: 1.5rem;
}

nav a {
  text-decoration: none;
  color: var(--text-light);
  font-weight: 500;
  padding: 0.5rem 0;
  position: relative;
  transition: color 0.3s;
}

nav a::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background-image: linear-gradient(45deg, var(--primary-color), var(--secondary-color));
  transition: width 0.3s ease-out;
}

nav a:hover,
nav a.router-link-exact-active {
  color: var(--primary-color);
}

nav a:hover::after,
nav a.router-link-exact-active::after {
  width: 100%;
}

main {
  flex-grow: 1; /* Powoduje, że główna treść rozpycha się, spychając stopkę na dół */
  width: 100%;
}

.app-footer {
  text-align: center;
  padding: 1.5rem;
  background-color: #343a40;
  color: #f8f9fa;
  font-size: 0.9rem;
  margin-top: auto; /* Przykleja stopkę do dołu, jeśli treść jest krótka */
}
</style>