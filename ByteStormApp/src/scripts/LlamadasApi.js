async function getOperativos(){
    let res = await fetch("https://localhost:7208/api/Operativo").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    return data
}

async function getMisiones(){
    let resM = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let dataM = await resM.json()
    return dataM
}

async function getEquipos(){
    let resE = await fetch("https://localhost:7208/api/Equipos").catch(error=>alert(`Error al cargar: ${error}`))
    let dataE = await resE.json()
    return dataE
}

async function postOperativo(aux){
    let json={
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch("https://localhost:7208/api/Operativo", json).catch(error=>alert(error)) 
}

async function deleteOperativo(id) {
    return await fetch(`https://localhost:7208/api/Operativo/${id}`, {
      method: 'DELETE',
    })
}

async function putOperativo(aux){
    let json={
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch(`https://localhost:7208/api/Operativo/${aux.id}`, json).catch(error=>alert(error))
}


async function postMision(aux){
    let json={
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch("https://localhost:7208/api/Misiones", json).catch(error=>alert(error)) 
}

async function deleteMision(id) {
    return await fetch(`https://localhost:7208/api/Misiones/${id}`, {
      method: 'DELETE',
    })
}

async function putMision(aux){
    let json={
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch(`https://localhost:7208/api/Misiones/${aux.id}`, json).catch(error=>alert(error))
}

async function postEquipo(aux){
    let json={
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch("https://localhost:7208/api/Equipos", json).catch(error=>alert(error)) 
}

async function deleteEquipo(id) {
    return await fetch(`https://localhost:7208/api/Equipos/${id}`, {
      method: 'DELETE',
    })
}

async function putEquipo(aux){
    let json={
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch(`https://localhost:7208/api/Equipos/${aux.id}`, json).catch(error=>alert(error))
}





export {getEquipos, getMisiones, getOperativos, postEquipo, deleteEquipo, putEquipo, postMision, deleteMision, putMision, postOperativo,deleteOperativo, putOperativo}