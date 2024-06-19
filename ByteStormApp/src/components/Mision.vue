<script setup>
import { ref, onMounted, reactive } from 'vue'

let newDescripcion = ref('')
let newEstado = ref('')
let newEquipos = ref('')
let editedDescripciones = []
let editedEstados = []
let editedEquipos = reactive([''])
const showEdit = ref([])

let misiones = reactive([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    misiones.value = data
})

async function addMision() {
  console.log(newEstado.value)
  let eq = (newEquipos.value) ? JSON.parse(`[${newEquipos.value.replaceAll(" ","")}]`) : []
  let aux = { descripcion: newDescripcion.value, estado: newEstado.value, equipos: eq}
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("https://localhost:7208/api/Misiones", json).catch(error=>alert(error))
  if(response.status == 201 || response.status==200){
    let data = await response.json()
    misiones.value.push(data)
  } else{
    alert("Error al crear mision")
  }
  newDescripcion.value=''
  newEstado.value=''
  newEquipos.value=''
}

async function removeMision(id) {
  const res = await fetch(`https://localhost:7208/api/Misiones/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    misiones.value = misiones.value.filter(o=>o.id != id)
  } else{
    alert("Error al eliminar mision")
  }
}

async function editMision(mision) {
  console.log(mision.estado)
  console.log(editedEstados[mision.id])
  let aux = { id: mision.id, 
    descripcion: (editedDescripciones[mision.id] ? editedDescripciones[mision.id] : mision.descripcion), 
    estado: (((editedEstados[mision.id] || editedEstados[mision.id]==0) && editedEstados[mision.id]!="") ? editedEstados[mision.id]: mision.estado),
    equipos: (editedEquipos[mision.id] ? JSON.parse(`[${editedEquipos[mision.id].replaceAll(" ","")}]`) : null)
  }
  console.log(aux.estado)
  let json={
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }
  let res = await fetch(`https://localhost:7208/api/Misiones/${mision.id}`, json).catch(error=>alert(error))
  
  if(res.status==204 || res.status==200){
    misiones.value.find((o)=>o.id==mision.id).descripcion = aux.descripcion
    misiones.value.find((o)=>o.id==mision.id).estado = aux.estado
    if(aux.equipos)
      misiones.value.find((o)=>o.id==mision.id).equipos.push(aux.equipos)
  } else{
    alert("Error al editar mision")
  }
  editedDescripciones[mision.id]=''
  editedEstados[mision.id]=''
  editedEquipos[mision.id]=''
}

const elems=[{title: 'Planificada', value:0}, {title: 'Activa', value:1}, {title: 'Completada', value:2}]

</script>

<template>
  <form @submit.prevent="addMision" class="d-flex flex-column align-center">
    <v-text-field v-model="newDescripcion" required placeholder="nuevo descripcion para mision" width="400"></v-text-field>
    <v-select v-model="newEstado" required :items="elems" density="comfortable" width="400"></v-select>
    <v-text-field v-model="newEquipos" placeholder="ids de las equipos de la mision" width="400"></v-text-field>
    <v-btn type="submit" class="mb-5">Añadir Mision</v-btn> 
  </form>

  <v-table height="315px" fixed-header>
    <thead>
      <tr>
        <th class="text-left">Id</th>
        <th class="text-left">Descripcion</th>
        <th class="text-left">Estado</th>
        <th class="text-left">Equipos</th>
        <th class="text-left">Id Operativo</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="mision in misiones.value":key="mision.id">
        <td>{{ mision.id }}</td>
        <td>{{ mision.descripcion }}</td>
        <td>{{ elems.at(mision.estado).title }}</td>
        <td>{{ mision.equipos.toString() }}</td>
        <td>{{ mision.idOperativo }}</td>
        <v-btn @click="removeMision(mision.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[mision.id] = !showEdit[mision.id]" class="bg-green" append-icon="mdi-pencil"> <!-- Hacer una nueva pag/componente para el edit -->
            <!-- <img src="@/assets/lapiz.png" alt="edit"> -->
          </v-btn>
          <td v-show="showEdit[mision.id]">
            <v-form @submit.prevent="editMision(mision)" v-show="showEdit[mision.id]">
            <v-text-field v-model="editedDescripciones[mision.id]" placeholder="Edita el descripcion de la mision" max-width="200"></v-text-field>
            <v-select v-model="editedEstados[mision.id]" :items="elems" density="comfortable" max-width="200"></v-select>
            <v-text-field v-model="editedEquipos[mision.id]" placeholder="Añade equipos a la mision" max-width="200"></v-text-field>
            <v-btn type="submit">Editar</v-btn> 
          </v-form>
          </td>
      </tr>
    </tbody>
  </v-table>

    <!-- <ul> 
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Descripcion</span>
          <span class="mr-5">Estado</span>
          <span class="mr-5">Equipos</span>
          <span class="mr-5">Id Operativo</span>
        </li>
        <li v-for="mision in misiones.value" :key="mision.id">
          <span class="mx-5">{{ mision.id }}</span>
          <span class="mr-5">{{ mision.descripcion }}</span>
          <span class="mr-5">{{ mision.estado }}</span>
          <span class="mr-5">{{ mision.equipos.toString() }}</span>
          <span class="mr-5">{{ mision.idOperativo }}</span>
          <v-btn @click="removeMision(mision.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[mision.id] = !showEdit[mision.id]" class="bg-green" append-icon="mdi-pencil"> Hacer una nueva pag/componente para el edit
           
          </v-btn>
          <v-form @submit.prevent="editMision(mision)" v-show="showEdit[mision.id]">
            <input v-model="editedDescripciones[mision.id]" placeholder="Edita el descripcion de la mision">
            <input v-model="editedEstados[mision.id]" placeholder="Edita el estado del mision">
            <input v-model="editedEquipos[mision.id]" placeholder="Añade equipos a la mision">
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
