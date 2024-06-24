<script setup>
import { ref, onMounted, reactive } from 'vue'

const elems=[{title: 'Planificada', value:0}, {title: 'Activa', value:1}, {title: 'Completada', value:2}]

let newDescripcion = ref('Nueva Mision')
let newEstado = ref(elems[0].value)
let newEquipos = ref([])
let editedDescripciones = ''
let editedEstados = ''
let editedEquipos = ref([])
const showEdit = reactive([])

const misiones = reactive([])
const nombreOperativos = ref()
const nombreEquipos = reactive([])
const equipos = reactive([]) 
onMounted(async () => {
    let res = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    data.forEach(item=>misiones.push(item))

    let resO = await fetch("https://localhost:7208/api/Operativo").catch(error=>alert(`Error al cargar: ${error}`))
    let dataO = await resO.json()
    nombreOperativos.value = dataO

    let resE = await fetch("https://localhost:7208/api/Equipos").catch(error=>alert(`Error al cargar: ${error}`))
    let dataE = await resE.json()
    nombreEquipos.value = dataE

    equipos.value = nombreEquipos.value.map(e=>{ return {
      title: e.descripcion,
      value: e.id
      };
    });
    console.log(equipos)

})

async function addMision() {
  let eq = (newEquipos.value) ? newEquipos.value : []
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

    if(aux.equipos){
      for(let e of aux.equipos){
        let index=misiones.find(m=>m.equipos.find(eq=>e==eq)).equipos.indexOf(e)
        misiones.find(m=>m.equipos.find(eq=>e==eq)).equipos.splice(index,1)
      }
    }
      

    misiones.push(data)
  } else{
    alert("Error al crear mision")
  }
  newDescripcion.value='Nueva Mision'
  newEstado.value=elems[0].value
  newEquipos.value=''
}

async function removeMision(id) {
  const res = await fetch(`https://localhost:7208/api/Misiones/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    misiones.splice(misiones.indexOf(misiones.find(o=>o.id==id)),1)
  } else{
    alert("Error al eliminar mision")
  }
}

async function editMision(mision) {
  let aux = { id: mision.id, 
    descripcion: (editedDescripciones ? editedDescripciones : mision.descripcion), 
    estado: (((editedEstados || editedEstados==0) && editedEstados!="") ? editedEstados: mision.estado),
    equipos: (editedEquipos.value ? editedEquipos.value : null)
  }
  let json={
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }
  let res = await fetch(`https://localhost:7208/api/Misiones/${mision.id}`, json).catch(error=>alert(error))
  
  if(res.status==204 || res.status==200){
    misiones.find((m)=>m.id==mision.id).descripcion = aux.descripcion
    misiones.find((m)=>m.id==mision.id).estado = aux.estado
    if(aux.equipos){
      for(let e of aux.equipos){
        let index=misiones.find(o=>o.equipos.find(eq=>e==eq)).equipos.indexOf(e)
        misiones.find(m=>m.equipos.find(eq=>eq==e)).equipos.splice(index,1)
        misiones.find((m)=>m.id==mision.id).equipos.push(e)
      }
    }
  } else{
    alert("Error al editar mision")
  }
  editedDescripciones=''
  editedEstados=''
  editedEquipos.value=[]
}


// for(let e of nombreEquipos.value){
//   equipos.push({
//     title: e.descripcion,
//     value: e.id
//   })
// }

// nombreEquipos.forEach((e)=>equipos.push({
//     title: e.descripcion,
//     value: e.id
//   }))

function obtenerNombresEquipos(idEquipos){
  let nombres = []
  for(let e of nombreEquipos.value){
    for(let id of idEquipos){
      if(e.id == id){
        nombres.push(e.descripcion)
      }
    }
  }
  return nombres
}

function obtenerOperativo(idOperativo){
  for(let o of nombreOperativos.value){
    if(o.id == idOperativo){
      return o.nombre
    }
  }
}

const numEdit=ref(0)

function mostrarEdit(id){
  numEdit.value=misiones.indexOf(misiones.find(o=>id==o.id))
  showEdit[numEdit.value] = true
}

</script>

<template>
  <form @submit.prevent="addMision" class="d-flex flex-column align-center">
    <v-text-field v-model="newDescripcion" required label="Nueva descripcion para mision" width="400"></v-text-field>
    <v-select v-model="newEstado" required :items="elems" density="comfortable" width="400" label="Elige el estado de la mision"></v-select>
    <v-select v-model="newEquipos" :items="equipos.value" density="comfortable" width="400" multiple label="Elige los equipos de la mision"></v-select>
    <v-btn type="submit" class="mb-5">Añadir Mision</v-btn> 
  </form>


  <v-table height="315px" fixed-header>
    <thead>
      <tr>
        <th class="text-left">Id</th>
        <th class="text-left">Descripcion</th>
        <th class="text-left">Estado</th>
        <th class="text-left">Equipos</th>
        <th class="text-left">Operativo</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="mision in misiones":key="mision.id">
        <td>{{ mision.id }}</td>
        <td>{{ mision.descripcion }}</td>
        <td>{{ elems.at(mision.estado).title }}</td>
        <td>{{ obtenerNombresEquipos(mision.equipos).toString() }}</td>
        <td>{{ obtenerOperativo(mision.idOperativo) }}</td>
        <v-btn @click="removeMision(mision.id)" class="ml-5 bg-red">X</v-btn> 
        <v-btn @click="mostrarEdit(mision.id)" class="bg-green" append-icon="mdi-pencil">
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
      <v-form @submit.prevent="editMision(misiones[numEdit])">
        <v-card-text>
          <v-text-field v-model="editedDescripciones" placeholder="Edita el descripcion de la mision"></v-text-field>
            <v-select v-model="editedEstados" :items="elems" density="comfortable"></v-select>
            <v-select v-model="editedEquipos" :items="equipos.value" density="comfortable" multiple></v-select>
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

    <!-- <ul> 
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
