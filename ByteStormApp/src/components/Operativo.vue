<script setup>
import { deleteOperativo, getMisiones, getOperativos, postOperativo, putOperativo } from '@/scripts/Operativo';
import { ref, onMounted, reactive, computed } from 'vue'

let newNombre = ref('Nuevo Operativo')
let newRol = ref('Nuevo Rol')
let newMisiones = ref([])
let editedNombres = ref('')
let editedRoles = ref('')
let editedMisiones = ref([])
const showEdit = reactive([])
const numEdit=ref(0)

const operativos = reactive([])
const nombreMisiones = reactive({})
const misiones = reactive([]) 
onMounted(async () => {
    let x=await getOperativos()
    x.forEach(item => operativos.push(item))
    nombreMisiones.value = await getMisiones()
    misiones.value = nombreMisiones.value.map(m=>{ return {
      title: m.descripcion,
      value: m.id
      };
    });
})

async function addOperativo() {
  let mis = (newMisiones.value) ? newMisiones.value : []
  let aux = { nombre: newNombre.value, rol: newRol.value, misiones: mis}
  

  let response = await postOperativo(aux)
  if(response.status == 201 || response.status==200){
    let data = await response.json()

    if(aux.misiones){
      for(let m of aux.misiones){
        let index=operativos.find(o=>o.misiones.find(mis=>m==mis)).misiones.indexOf(m)
        operativos.find(o=>o.misiones.find(mis=>m==mis)).misiones.splice(index,1)
      }
    }

    operativos.push(data)
  } else{
    alert("Error al crear operativo")
  }
  newNombre.value='Nuevo Operativo'
  newRol.value='Nuevo Rol'
  newMisiones.value=[]
}

async function removeOperativo(id) {
  const res = await deleteOperativo(id)
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    
    operativos.splice(operativos.indexOf(operativos.find(o=>o.id==id)),1)
  } else{
    alert("Error al eliminar operativo")
  }
}

async function editOperativo(operativo) {
  let aux = { id: operativo.id, 
    nombre: (editedNombres.value ? editedNombres.value : operativo.nombre), 
    rol: (editedRoles.value ? editedRoles.value : operativo.rol),
    misiones: (editedMisiones.value ? editedMisiones.value : null)
  }
  
  let res = await putOperativo(aux)
  
  if(res.status==204 || res.status==200){
    operativos.find((o)=>o.id==operativo.id).nombre = aux.nombre
    operativos.find((o)=>o.id==operativo.id).rol = aux.rol
    if(aux.misiones){
      for(let m of aux.misiones){
        let index=operativos.find(o=>o.misiones.find(mis=>m==mis)).misiones.indexOf(m)
        operativos.find(o=>o.misiones.find(mis=>m==mis)).misiones.splice(index,1)
        operativos.find((o)=>o.id==operativo.id).misiones.push(m)
      }
    }
  } else{
    alert("Error al editar operativo")
  }
  editedNombres.value=''
  editedRoles.value=''
  editedMisiones.value=[]
}

function obtenerNombres(idMisiones){
  let nombres = []
  for(let m of nombreMisiones.value){
    for(let id of idMisiones){
      if(m.id == id){
        nombres.push(m.descripcion)
      }
    }
  }
  return nombres
}


function mostrarEdit(id){
  numEdit.value=operativos.indexOf(operativos.find(o=>id==o.id))
  showEdit[numEdit.value] = true
}



</script>

<template>
  <form @submit.prevent="addOperativo" class="d-flex flex-column align-center">
    <v-text-field v-model="newNombre" required label="nuevo nombre para operativo" width="400"></v-text-field>
    <v-text-field v-model="newRol" required label="nuevo rol para operativo" width="400"></v-text-field>
    <v-select v-model="newMisiones" :items="misiones.value" density="comfortable" width="400" multiple label="Elige las misiones del operativo"></v-select>
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
      <tr v-for="operativo in operativos":key="operativo.id">
        <td>{{ operativo.id }}</td>
        <td>{{ operativo.nombre }}</td>
        <td>{{ operativo.rol }}</td>
        <td>{{ obtenerNombres(operativo.misiones).toString() }}</td>

        <v-btn @click="removeOperativo(operativo.id)" class="ml-5 bg-red">X</v-btn> 
        <v-btn @click="mostrarEdit(operativo.id)" class="bg-green" append-icon="mdi-pencil">
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
      <v-form @submit.prevent="editOperativo(operativos[numEdit])">
        <v-card-text>
            <v-text-field v-model="editedNombres" placeholder="Edita el nombre del operativo"></v-text-field>
            <v-text-field v-model="editedRoles" placeholder="Edita el rol del operativo"></v-text-field>
            <v-select v-model="editedMisiones" :items="misiones.value" density="comfortable" multiple label="Añade misiones del operativo"></v-select>
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
