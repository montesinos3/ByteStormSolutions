<script setup>
import { ref, onMounted, reactive } from 'vue'

let newDescripcion = ref('')
let newEstado = ref('')
let newEquipos = ref('')
let editedDescripciones = []
let editedEstados = []
let editedEquipos = ['']
const showEdit = ref([])

let misiones = ref([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    misiones.value = data
})

async function addMision() {
  let eq = (newEquipos.value) ? JSON.parse(`[${newEquipos.value.replaceAll(" ","")}]`) : []
  let aux = { descripcion: newDescripcion.value, estado: parseInt(newEstado.value,10), equipos: eq}
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
  console.log(mision.descripcion)
  console.log(editedDescripciones[mision.id])
  let aux = { id: mision.id, 
    descripcion: (editedDescripciones[mision.id] ? editedDescripciones[mision.id] : mision.descripcion), 
    estado: (editedEstados[mision.id] ? parseInt(editedEstados[mision.id],10) : parseInt(mision.estado,10)),
    equipos: (editedEquipos[mision.id] ? JSON.parse(`[${editedEquipos[mision.id].replaceAll(" ","")}]`) : mision.equipos)
  }
  console.log(mision.descripcion)
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
    misiones.value.find((o)=>o.id==mision.id).equipos.push(aux.equipos)
  } else{
    alert("Error al editar mision")
  }
  editedDescripciones[mision.id]=''
  editedEstados[mision.id]=''
  editedEquipos[mision.id]=''
}

</script>

<template>
  <form @submit.prevent="addMision" class="ma-5">
    <v-text-field v-model="newDescripcion" required placeholder="nuevo descripcion para mision" max-width="400"></v-text-field>
    <v-text-field v-model="newEstado" required placeholder="estado [Planificada(0), Activa(1), Completada(2)]" max-width="400"></v-text-field>
    <v-text-field v-model="newEquipos" placeholder="ids de las equipos de la mision" max-width="400"></v-text-field>
    <v-btn type="submit" class="mb-5">Añadir Mision</v-btn> 
  </form>
    <ul> <!-- Probar a hacer una tabla -->
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Descripcion</span>
          <span class="mr-5">Estado</span>
          <span class="mr-5">Equipos</span>
          <span class="mr-5">Id Operativo</span>
        </li>
        <li v-for="mision in misiones" :key="mision.id">
          <span class="mx-5">{{ mision.id }}</span>
          <span class="mr-5">{{ mision.descripcion }}</span>
          <span class="mr-5">{{ mision.estado }}</span>
          <span class="mr-5">{{ mision.equipos.toString() }}</span>
          <span class="mr-5">{{ mision.idOperativo }}</span>
          <v-btn @click="removeMision(mision.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[mision.id] = !showEdit[mision.id]" class="bg-green" append-icon="mdi-pencil"> <!-- Hacer una nueva pag/componente para el edit -->
            <!-- <img src="@/assets/lapiz.png" alt="edit"> -->
          </v-btn>
          <v-form @submit.prevent="editMision(mision)" v-show="showEdit[mision.id]">
            <input v-model="editedDescripciones[mision.id]" placeholder="Edita el descripcion de la mision">
            <input v-model="editedEstados[mision.id]" placeholder="Edita el estado del mision">
            <input v-model="editedEquipos[mision.id]" placeholder="Añade equipos a la mision">
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
