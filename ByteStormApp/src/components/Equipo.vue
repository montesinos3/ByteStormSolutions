<script setup>
import { ref, onMounted, reactive, computed } from 'vue'

const es = [{title:'Disponible', value:0}, {title:'En Uso', value:1}]
const ts = [{title:'Software', value:0}, {title:'Hardware', value:1}]

let dialog=ref(false)
let newTipo = ref(ts[0].value)
let newDescripcion = ref('Nuevo Equipo')
let newEstado = ref(es[0].value)
let editedTipos = ''
let editedDescripciones = ''
let editedEstados = ''
const showEdit = reactive([])

let equipos = reactive([])
const nombreMisiones = reactive([])
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Equipos").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    data.forEach(item => equipos.push(item));

    let resM = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let dataM = await resM.json()
    nombreMisiones.value = dataM
})

async function addEquipo() {
  let aux = { descripcion: newDescripcion.value, tipo: newTipo.value, estado: newEstado.value}
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("https://localhost:7208/api/Equipos", json).catch(error=>alert(error))
  if(response.status == 201 || response.status==200){
    let data = await response.json()
    equipos.push(data)
  } else{
    alert("Error al crear equipo")
  }
  newDescripcion.value='Nuevo Equipo'
  newEstado.value=es[0].value
  newTipo.value=ts[0].value
}

async function removeEquipo(id) {
  const res = await fetch(`https://localhost:7208/api/Equipos/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    equipos.splice(equipos.indexOf(equipos.find(o=>o.id==id)),1)
  } else{
    alert("Error al eliminar equipo")
  }
}

async function editEquipo(equipo) {
  let aux = { id: equipo.id, 
    descripcion: (editedDescripciones ? editedDescripciones : equipo.descripcion), 
    tipo: (((editedTipos || editedTipos==0 )&& editedTipos!="") ? editedTipos: equipo.tipo),
    estado: (((editedEstados || editedEstados==0) && editedEstados!="") ? editedEstados : equipo.estado)
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
    equipos.find((o)=>o.id==equipo.id).descripcion = aux.descripcion
    equipos.find((o)=>o.id==equipo.id).estado = aux.estado
    equipos.find((o)=>o.id==equipo.id).tipo = aux.tipo
  } else{
    alert("Error al editar equipo")
  }
  editedDescripciones=''
  editedEstados=''
  editedTipos=''
}

function obtenerMision(idMision){
  for(let m of nombreMisiones.value){
    if(m.id == idMision){
      return m.descripcion
    }
  }
}
const numEdit=ref(0)

function mostrarEdit(id){
  numEdit.value=equipos.indexOf(equipos.find(o=>id==o.id))
  showEdit[numEdit.value] = true
}

</script>

<template>
  <form @submit.prevent="addEquipo" class="d-flex flex-column align-center">
    <v-text-field v-model="newDescripcion" required label="Nueva descripcion para equipo" width="400"></v-text-field>
    <v-select v-model="newEstado" required :items="es" density="comfortable" width="400" label="Elige el estado del equipo"></v-select>
    <v-select v-model="newTipo" required :items="ts" density="comfortable" width="400" label="Elige el tipo de equipo"></v-select>
    <v-btn type="submit" class="mb-5">Añadir Equipo</v-btn> 
  </form>

  <v-table height="315px" fixed-header>
    <thead>
      <tr>
        <th class="text-left">Id</th>
        <th class="text-left">Descripcion</th>
        <th class="text-left">Estado</th>
        <th class="text-left">Tipo</th>
        <th class="text-left">Mision</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="equipo in equipos" :key="equipo.id">
        <td>{{ equipo.id }}</td>
        <td>{{ equipo.descripcion }}</td>
        <td>{{ es.at(equipo.estado).title }}</td>
        <td>{{ ts.at(equipo.tipo).title }}</td>
        <td>{{ obtenerMision(equipo.idMision) }}</td>
        <v-btn @click="removeEquipo(equipo.id)" class="ml-5 bg-red">X</v-btn> 
          <v-btn @click="mostrarEdit(equipo.id)" class="bg-green" append-icon="mdi-pencil">
          </v-btn>
      </tr>
    </tbody>
  </v-table>
<div>
    <v-dialog
        v-model="showEdit[numEdit]"
        max-width="600"
    >
      <v-card
        prepend-icon="mdi-account"
        title="User Profile"
      >
      <v-form @submit.prevent="editEquipo(equipos[numEdit])">
        <v-card-text>
            <v-text-field v-model="editedDescripciones" placeholder="Edita la descripcion del equipo"></v-text-field>
            <v-select v-model="editedEstados" :items="es" density="comfortable"></v-select>
            <v-select v-model="editedTipos" :items="ts" density="comfortable"></v-select>
          </v-card-text>
          
          <v-divider></v-divider>
          
        <v-card-actions>
          <v-spacer></v-spacer>

          <v-btn
          text="Close"
          variant="plain"
          @click="showEdit[numEdit]=false"
          ></v-btn>
          
          <v-btn
            type="submit"
            color="primary"
            text="Save"
            variant="tonal"
            @click="showEdit[numEdit]=false"
          ></v-btn>
        </v-card-actions>
      </v-form>
      </v-card>
    </v-dialog>
</div>

<!-- 
    <ul>  Probar a hacer una tabla 
        <li>
          <span class="mx-5">Id</span>
          <span class="mr-5">Descripcion</span>
          <span class="mr-5">Tipo</span>
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
          <v-btn @click="showEdit[equipo.id] = !showEdit[equipo.id]" class="bg-green" append-icon="mdi-pencil">  Hacer una nueva pag/componente para el edit
             <img src="@/assets/lapiz.png" alt="edit">
          </v-btn>
          <v-form @submit.prevent="editEquipo(equipo)" v-show="showEdit[equipo.id]">
            <input v-model="editedDescripciones[equipo.id]" placeholder="Edita la descripcion del equipo">
            <input v-model="editedEstados[equipo.id]" placeholder="Edita el estado del equipo (0,1)">
            <input v-model="editedTipos[equipo.id]" placeholder="Edita el tipo del equipo (0,1)">
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
