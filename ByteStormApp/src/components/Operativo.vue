<script setup>
import { ref, onMounted, reactive } from 'vue'

let newNombre = ref('')
let newRol = ref('')
let newMisiones = ref('')
let editedNombres = []
let editedRoles = []
let editedMisiones = reactive([''])
const showEdit = ref([])

const operativos = reactive([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Operativo").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    operativos.value = data
})

async function addOperativo() {
  let mis = (newMisiones.value) ? JSON.parse(`[${newMisiones.value.replaceAll(" ","")}]`) : []
  let aux = { nombre: newNombre.value, rol: newRol.value, misiones: reactive(mis)}
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("https://localhost:7208/api/Operativo", json).catch(error=>alert(error))
  if(response.status == 201 || response.status==200){
    let data = await response.json()
    operativos.value.push(data)
  } else{
    alert("Error al crear operativo")
  }
  newNombre.value=''
  newRol.value=''
  newMisiones.value=''
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
    nombre: (editedNombres[operativo.id] ? editedNombres[operativo.id] : operativo.nombre), 
    rol: (editedRoles[operativo.id] ? editedRoles[operativo.id] : operativo.rol),
    misiones: (editedMisiones[operativo.id] ? JSON.parse(`[${editedMisiones[operativo.id].replaceAll(" ","")}]`) : null)
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
    if(aux.misiones)
      operativos.value.find((o)=>o.id==operativo.id).misiones.push(aux.misiones)
  } else{
    alert("Error al editar operativo")
  }
  editedNombres[operativo.id]=''
  editedRoles[operativo.id]=''
  editedMisiones[operativo.id]=''
}

</script>

<template>
  <form @submit.prevent="addOperativo" class="d-flex flex-column align-center">
    <v-text-field v-model="newNombre" required placeholder="nuevo nombre para operativo" width="400"></v-text-field>
    <v-text-field v-model="newRol" required placeholder="nuevo rol para operativo" width="400"></v-text-field>
    <v-text-field v-model="newMisiones" placeholder="ids de las misiones del operativo" width="400"></v-text-field>
    <v-btn type="submit" class="mb-5">Añadir Operativo</v-btn> 
  </form>


  <v-table height="315px" fixed-header>
    <thead>
      <tr>
        <th class="text-left">Id</th>
        <th class="text-left">Nombre</th>
        <th class="text-left">Rol</th>
        <th class="text-left">Misiones</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="operativo in operativos.value":key="operativo.id">
        <td>{{ operativo.id }}</td>
        <td>{{ operativo.nombre }}</td>
        <td>{{ operativo.rol }}</td>
        <td>{{ operativo.misiones.toString() }}</td>

        <v-btn @click="removeOperativo(operativo.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[operativo.id] = !showEdit[operativo.id]" class="bg-green" append-icon="mdi-pencil"></v-btn>
          <td v-show="showEdit[operativo.id]">
            <v-form @submit.prevent="editOperativo(operativo)" v-show="showEdit[operativo.id]">
            <input v-model="editedNombres[operativo.id]" placeholder="Edita el nombre del operativo">
            <input v-model="editedRoles[operativo.id]" placeholder="Edita el rol del operativo">
            <input v-model="editedMisiones[operativo.id]" placeholder="Añade misiones al operativo">
            <v-btn type="submit">Editar</v-btn> 
            </v-form>
          </td>
      </tr>
    </tbody>
  </v-table>




    <!--<ul>  Probar a hacer una tabla 
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Nombre</span>
          <span class="mr-5">Rol</span>
          <span class="mr-5">Misiones</span>
        </li>
        <li v-for="operativo in operativos.value" :key="operativo.id">
          <span class="mx-5">{{ operativo.id }}</span>
          <span class="mr-5">{{ operativo.nombre }}</span>
          <span class="mr-5">{{ operativo.rol }}</span>
          <span class="mr-5">{{ operativo.misiones.toString() }}</span>
          <v-btn @click="removeOperativo(operativo.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[operativo.id] = !showEdit[operativo.id]" class="bg-green" append-icon="mdi-pencil"> Hacer una nueva pag/componente para el edit
            <img src="@/assets/lapiz.png" alt="edit">
          </v-btn>
          <v-form @submit.prevent="editOperativo(operativo)" v-show="showEdit[operativo.id]">
            <input v-model="editedNombres[operativo.id]" placeholder="Edita el nombre del operativo">
            <input v-model="editedRoles[operativo.id]" placeholder="Edita el rol del operativo">
            <input v-model="editedMisiones[operativo.id]" placeholder="Añade misiones al operativo">
            <v-btn type="submit">Editar</v-btn> 
          </v-form>
        </li>
    </ul> -->
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
