<template>
  <div class="container">
    <header class="hero">
      <h1>Witaj w Projekcie E-Commerce</h1>
      <p class="subtitle">Nowoczesna aplikacja do zarządzania produktami i zamówieniami w .NET i Vue.js.</p>
    </header>

    <div class="features">
      <div class="card-container">
        <RouterLink to="/products" class="feature-card">
          <div class="icon">📦</div>
          <h2>Zarządzaj Produktami</h2>
          <p>Dodawaj, edytuj i usuwaj produkty dostępne w systemie.</p>
        </RouterLink>

        <RouterLink to="/orders" class="feature-card">
          <div class="icon">🛒</div>
          <h2>Zarządzaj Zamówieniami</h2>
          <p>Przeglądaj, twórz i edytuj zamówienia klientów.</p>
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup>
import { RouterLink } from 'vue-router'
</script>

<style scoped>
.hero {
  text-align: center;
  padding: 4rem 0;
}
.hero h1 {
  font-size: 3rem;
  margin-bottom: 1rem;
}
.hero .subtitle {
  font-size: 1.25rem;
  color: var(--text-light);
  max-width: 600px;
  margin: 0 auto 3rem auto;
}
.card-container {
  display: flex;
  justify-content: center;
  gap: 2rem;
  flex-wrap: wrap;
}
.feature-card {
  background: var(--card-background);
  border-radius: var(--border-radius);
  padding: 2.5rem;
  width: 320px;
  text-decoration: none;
  color: inherit;
  box-shadow: var(--box-shadow);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  text-align: center;
  border-top: 4px solid var(--primary-color);
}
.feature-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
}
.feature-card .icon {
  font-size: 4rem;
  margin-bottom: 1.5rem;
  line-height: 1;
}
.feature-card h2 {
  font-size: 1.5rem;
  color: var(--text-color);
  margin-bottom: 0.75rem;
  border: none;
}
.feature-card p {
  font-size: 1rem;
  color: var(--text-light);
}
</style>