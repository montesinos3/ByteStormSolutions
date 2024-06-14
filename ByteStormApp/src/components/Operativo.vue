<script setup>
import { ref, onMounted } from 'vue'

let newNombre = ref('')
let newRol = ref('')
let newMision = ref('')
let editedNombres = []
let editedRoles = []
const showEdit = ref([])

let operativos = ref([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Operativo").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    operativos.value = data
})

async function addOperativo() {
  let aux = { nombre: newNombre.value, rol: newRol.value}
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("https://localhost:7208/api/Operativo", json).catch(error=>alert(error))
  let data = await response.json()
  operativos.value.push(data)
  newNombre.value=''
  newRol.value=''
}

async function removeOperativo(id) {
  const res = await fetch(`https://localhost:7208/api/Operativo/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    operativos.value = operativos.value.filter(o=>o.id != id)
  } else{
    alert("Error al eliminar operativo")
  }
}

async function editOperativo(operativo) {
  let aux = { id: operativo.id, 
    nombre: (editedNombres[operativo.id]!='') ? editedNombres[operativo.id] : operativo.nombre, 
    rol: (editedRoles[operativo.id]!='') ? editedRoles[operativo.id] : operativo.rol
  }
  let json={
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }
  let res = await fetch(`https://localhost:7208/api/Operativo/${operativo.id}`, json).catch(error=>alert(error))
  
  if(res.status==204 || res.status==200){
    operativos.value.find((o)=>o.id==operativo.id).nombre = aux.nombre
    operativos.value.find((o)=>o.id==operativo.id).rol = aux.rol
  } else{
    alert("Error al editar operativo")
  }
  editedNombres[operativo.id]=''
  editedRoles[operativo.id]=''
}

</script>

<template>
  <v-form @submit.prevent="addOperativo">
    <input class="mx-5 w-25 px-4 bg-grey-darken-1" v-model="newNombre" required placeholder="nuevo nombre para operativo">
    <input class="mx-5 px-4 bg-grey-darken-1" v-model="newRol" required placeholder="nuevo rol para operativo">
    <v-btn type="submit">Añadir Operativo</v-btn> 
  </v-form>
    <ul> <!-- Probar a hacer una tabla -->
        <li>
          <span class="mr-5">Nombre</span>
          <span class="mr-5">Rol</span>
        </li>
        <li v-for="operativo in operativos" :key="operativo.id">
        <span class="mr-5">{{ operativo.nombre }}</span>
        <span class="mr-5">{{ operativo.rol }}</span>
        <v-btn @click="removeOperativo(operativo.id)" class="ml-5 bg-red">X</v-btn> 
        <v-btn @click="showEdit[operativo.id] = !showEdit[operativo.id]" class="bg-green"> <!-- Hacer una nueva pag/componente para el edit -->
          <img src="@/assets/lapiz.png" alt="edit">
        </v-btn>
        <v-form @submit.prevent="editOperativo(operativo)" v-show="showEdit[operativo.id]">
          <input v-model="editedNombres[operativo.id]" placeholder="Edita el nombre del operativo">
          <input v-model="editedRoles[operativo.id]" placeholder="Edita el rol del operativo">
          <v-btn type="submit">Editar</v-btn> 
        </v-form>
        </li>
    </ul>
</template>

<style scoped>
.done {
  text-decoration: line-through;
}
img{
  width:20px;
  height: 20px;
}
ul{
  background-color: rgb(155, 155, 150);
  width: fit-content;
  margin-left: 20px;
  list-style: none;
  padding-left: 0%;
}
</style>
