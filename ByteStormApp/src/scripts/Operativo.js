export async function getOperativos(){
    let res = await fetch("https://localhost:7208/api/Operativo").catch(error=>alert(`Error al cargar: ${error}`))
    let data = await res.json()
    return data
}

export async function getMisiones(){
    let resM = await fetch("https://localhost:7208/api/Misiones").catch(error=>alert(`Error al cargar: ${error}`))
    let dataM = await resM.json()
    return dataM
}

export async function postOperativo(aux){
    let json={
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch("https://localhost:7208/api/Operativo", json).catch(error=>alert(error)) 
}

export async function deleteOperativo(id) {
    return await fetch(`https://localhost:7208/api/Operativo/${id}`, {
      method: 'DELETE',
    })
}

export async function putOperativo(aux){
    let json={
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(aux)
    }
    return await fetch(`https://localhost:7208/api/Operativo/${aux.id}`, json).catch(error=>alert(error))
}