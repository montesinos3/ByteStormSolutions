<template>
      <v-app-bar
        color="primary"
        prominent
      >
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>

        <v-toolbar-title>My files</v-toolbar-title>

        <v-spacer></v-spacer>

        <template v-if="$vuetify.display.mdAndUp">
          <v-btn icon="mdi-magnify" variant="text"></v-btn>

          <v-btn icon="mdi-filter" variant="text"></v-btn>
        </template>

        <v-btn icon="mdi-dots-vertical" variant="text"></v-btn>
      </v-app-bar>

      <v-navigation-drawer v-model="drawer" :location="undefined" temporary>
        <v-list>
          <v-list-item v-for="item in items" :key="item.value">
            <router-link :to="item.to">
                <v-btn>
                <v-list-item-content>
                  <v-list-item-title>{{ item.title }}</v-list-item-title>
                </v-list-item-content>
              </v-btn>
            </router-link>
          </v-list-item>
        </v-list>
      </v-navigation-drawer>
</template>

<script setup>
import { ref, reactive, watch } from 'vue'

const drawer = ref(false)
const group = ref(null)
const items = reactive([
  {
    title: 'Operativos',
    value: 'Operativos',
    to: "/Operativo",
  },
  {
    title: 'Misiones',
    value: 'Misiones',
    to: "/Mision",
  },
  {
    title: 'Equipos',
    value: 'Equipos',
    to: "/Equipo",
  },
])

watch(group, () => {
  drawer.value = false
})
</script>