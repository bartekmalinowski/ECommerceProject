<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="wrapper">
      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/products">Zarządzaj Produktami</RouterLink>
        <!-- NOWOŚĆ: Link do zarządzania zamówieniami -->
        <RouterLink to="/orders">Zarządzaj Zamówieniami</RouterLink> 
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style scoped>
/* ... style bez zmian ... */
header { line-height: 1.5; }
nav { width: 100%; font-size: 12px; text-align: center; margin-top: 2rem; }
nav a { display: inline-block; padding: 0 1rem; border-left: 1px solid var(--color-border); }
nav a:first-of-type { border: 0; }
</style>