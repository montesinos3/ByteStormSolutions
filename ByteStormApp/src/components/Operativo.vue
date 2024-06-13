<script setup>
import { ref, computed, onMounted } from 'vue'

let newNombre = ref('')
let newRol = ref('')
let newMision = ref('')
let OperativosEditados = []
const showEdit = ref([])

let todos = ref([])
onMounted(async () => {
    let res = await fetch("http://localhost:7208/api/Operativos").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    todos.value = data
})

 let filteredTodos = computed(() => {
    return hideCompleted.value
      ? todos.value.filter((t) => !t.isComplete)
      : todos.value
 })

async function addTodo() {
  let aux = { name: newTodo.value, isComplete: false }
  let json={
    method: 'POST',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }

  let response = await fetch("http://localhost:7208/api/Operativos", json).catch(error=>alert(error))
  let data = await response.json()
  todos.value.push(data)
  newTodo.value=''
}

async function removeTodo(id) {
  const res = await fetch(`http://localhost:7208/api/Operativos/${id}`, {
    method: 'DELETE',
  })
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    todos.value = todos.value.filter(t=>t.id != id)
  } else{
    alert("Error al eliminar todo")
  }
}

async function editTodo(todo) {
  let aux = { id: todo.id, name: editedTodo[todo.id], isComplete: todo.isComplete}
  let json={
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json;charset=utf-8'
    },
    body: JSON.stringify(aux)
  }
  
  console.log("El antiguo nombre es: ", todo.name)
  console.log("El nuevo nombre es: ", editedTodo[todo.id])
  let res = await fetch(`http://localhost:7208/api/Operativos/${todo.id}`, json).catch(error=>alert(error))
  
  if(res.status==204 || res.status==200){
    //eliminar todo por id
    // let index=todos.value.findIndex((t)=>t.id==todo.id)
    // todos.value.splice(index,1)
    //insertar el nuevo todo
    // todos.push(data)
    todos.value.find((t)=>t.id==todo.id).name = editedTodo[todo.id]
    console.log("Se ha cambiado el todo.value: ", todos.value.find((t)=>t.id==todo.id).value.name)
  } else{
    alert("Error al editar todo")
  }
  editedTodo[todo.id]=''
}

</script>

<template>
  <form @submit.prevent="addTodo">
    <input v-model="newTodo" required placeholder="new todo">
    <button>Add Todo</button> 
  </form>
    <ul>
        <li v-for="todo in filteredTodos" :key="todo.id">
        <input type="checkbox" v-model="todo.isComplete"> 
        <span :class="{ done: todo.isComplete }">{{ todo.name }}</span> 
        <button @click="removeTodo(todo.id)">X</button> 
        <button @click="showEdit[todo.id] = !showEdit[todo.id]">
          <img src="../../icons/lapiz.png" alt="edit">
        </button> 
        <form @submit.prevent="editTodo(todo)" v-show="showEdit[todo.id]">
          <input v-model="editedTodo[todo.id]" required placeholder="Edit your todo" > 
          
          <button>Edit Todo</button> 
        </form>
        
        </li>
    </ul>
    <button @click="hideCompleted = !hideCompleted">
        {{ hideCompleted ? 'Show all' : 'Hide completed' }}
    </button>
</template>

<style scoped>
.done {
  text-decoration: line-through;
}
img{
  width:12px;
  height: 12px;
}
</style>
