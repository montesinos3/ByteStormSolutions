/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router'
import { setupLayouts } from 'virtual:generated-layouts'
import Operativo from '@/components/Operativo.vue'
import Mision from '@/components/Mision.vue'
import Equipo from '@/components/Equipo.vue'
import App from '@/App.vue'
import Ejemplo from '@/pages/ejemplo.vue'

const routes = [
  { path: '/Operativo', component: Operativo},
  { path: '/Equipo', component: Equipo },
  { path: '/Mision', component: Mision },
  { path: '/Home', component: Ejemplo }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  // extendRoutes: setupLayouts,
  routes
})
// router.addRoute({ path: '/Operativo', component: Operativo })
export default router
