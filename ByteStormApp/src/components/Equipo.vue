<script setup>
import { ref, onMounted, reactive } from 'vue'

let newTipo = ref('')
let newDescripcion = ref('')
let newEstado = ref('')
let editedTipos = ['']
let editedDescripciones = []
let editedEstados = []
const showEdit = ref([])

let equipos = ref([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Equipos").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    equipos.value = data
})

async function addEquipo() {
  let aux = { descripcion: newDescripcion.value, tipo: parseInt(newTipo.value,10), estado: parseInt(newEstado.value,10)}
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("https://localhost:7208/api/Equipos", json).catch(error=>alert(error))
  let data = await response.json()
  equipos.value.push(data)
  newDescripcion.value=''
  newEstado.value=''
  newTipo.value=''
}

async function removeEquipo(id) {
  const res = await fetch(`https://localhost:7208/api/Equipos/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    equipos.value = equipos.value.filter(o=>o.id != id)
  } else{
    alert("Error al eliminar equipo")
  }
}

async function editEquipo(equipo) {
  let aux = { id: equipo.id, 
    descripcion: (((editedDescripciones[equipo.id]!=undefined) && (editedDescripciones[equipo.id]!="")) ? editedDescripciones[equipo.id] : equipo.descripcion), 
    tipo: (((editedTipos[equipo.id]!=undefined) && (editedTipos[equipo.id]!="")) ? parseInt(editedTipos[equipo.id],10) : parseInt(equipo.descripcion,10)),
    estado: (((editedEstados[equipo.id]!=undefined) && (editedEstados[equipo.id]!="")) ? parseInt(editedEstados[equipo.id],10) : parseInt(equipo.estado,10))
  }
  let json={
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }
  let res = await fetch(`https://localhost:7208/api/Equipos/${equipo.id}`, json).catch(error=>alert(error))
  
  if(res.status==204 || res.status==200){
    equipos.value.find((o)=>o.id==equipo.id).descripcion = aux.descripcion
    equipos.value.find((o)=>o.id==equipo.id).estado = aux.estado
    equipos.value.find((o)=>o.id==equipo.id).tipo = aux.tipo
  } else{
    alert("Error al editar equipo")
  }
  editedDescripciones[equipo.id]=''
  editedEstados[equipo.id]=''
  editedTipos[equipo.id]=''
}

</script>

<template>
  <form @submit.prevent="addEquipo" class="ma-5">
    <v-text-field v-model="newDescripcion" required placeholder="nueva descripcion para equipo" max-width="400"></v-text-field>
    <v-text-field v-model="newEstado" required placeholder="estado [Disponible(0), En uso(1)]" max-width="400"></v-text-field>
    <v-text-field v-model="newTipo" required placeholder="tipo [Software(0), Hardware(1)]" max-width="400"></v-text-field>
    <v-btn type="submit" class="mb-5">Añadir Equipo</v-btn> 
  </form>
    <ul> <!-- Probar a hacer una tabla -->
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Descripcion</span>
          <span class="mr-5">Estado</span>
          <span class="mr-5">Mision</span>
        </li>
        <li v-for="equipo in equipos" :key="equipo.id">
          <span class="mx-5">{{ equipo.id }}</span>
          <span class="mr-5">{{ equipo.descripcion }}</span>
          <span class="mr-5">{{ equipo.tipo }}</span>
          <span class="mr-5">{{ equipo.estado }}</span>
          <span class="mr-5">{{ equipo.idMision }}</span>
          <v-btn @click="removeEquipo(equipo.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="showEdit[equipo.id] = !showEdit[equipo.id]" class="bg-green" append-icon="mdi-pencil"> <!-- Hacer una nueva pag/componente para el edit -->
            <!-- <img src="@/assets/lapiz.png" alt="edit"> -->
          </v-btn>
          <v-form @submit.prevent="editEquipo(equipo)" v-show="showEdit[equipo.id]">
            <input v-model="editedDescripciones[equipo.id]" placeholder="Edita la descripcion del equipo">
            <input v-model="editedEstados[equipo.id]" placeholder="Edita el estado del equipo (0,1)">
            <input v-model="editedTipos[equipo.id]" placeholder="Edita el tipo del equipo (0,1)">
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
