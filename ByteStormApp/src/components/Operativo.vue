<script setup>
import { ref, onMounted, reactive } from 'vue'

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
    nombre: (((editedNombres[operativo.id]!=undefined) && (editedNombres[operativo.id]!="")) ? editedNombres[operativo.id] : operativo.nombre), 
    rol: (((editedRoles[operativo.id]!=undefined) && (editedRoles[operativo.id]!="")) ? editedRoles[operativo.id] : operativo.rol)
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
  <form @submit.prevent="addOperativo" class="ma-5">
    <v-text-field v-model="newNombre" required placeholder="nuevo nombre para operativo" max-width="300"></v-text-field>
    <v-text-field v-model="newRol" required placeholder="nuevo rol para operativo" max-width="300"></v-text-field>
    <!-- <v-text-field v-model="newMision" placeholder="id de la mision del operativo" max-width="300"></v-text-field> -->
    <v-btn type="submit" class="mb-5">Añadir Operativo</v-btn> 
  </form>
    <ul> <!-- Probar a hacer una tabla -->
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Nombre</span>
          <span class="mr-5">Rol</span>
          <!-- <span class="mr-5">Mision</span> -->
        </li>
        <li v-for="operativo in operativos" :key="operativo.id">
          <span class="mx-5">{{ operativo.id }}</span>
          <span class="mr-5">{{ operativo.nombre }}</span>
          <span class="mr-5">{{ operativo.rol }}</span>
          <!-- <span class="mr-5">{{ operativo.mision }}</span> -->
          <v-btn @click="removeOperativo(operativo.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[operativo.id] = !showEdit[operativo.id]" class="bg-green" append-icon="mdi-pencil"> <!-- Hacer una nueva pag/componente para el edit -->
            <!-- <img src="@/assets/lapiz.png" alt="edit"> -->
          </v-btn>
          <v-form @submit.prevent="editOperativo(operativo)" v-show="showEdit[operativo.id]">
            <input v-model="editedNombres[operativo.id]" placeholder="Edita el nombre del operativo">
            <input v-model="editedRoles[operativo.id]" placeholder="Edita el rol del operativo">
            <!-- <input v-model="editedMisiones[operativo.id]" placeholder="Edita la mision del operativo"> -->
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
