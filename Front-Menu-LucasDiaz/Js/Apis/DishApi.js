const API_BASE = "https://localhost:7036/api/v1/Dish"; // Reemplaza con la URL base de tu API
export async function getDishes() {
    try {
        const response = await axios.get(API_BASE);
        if(response.data && Array.isArray(response.data)) 
        {
            return response.data;
        } else {
            console.warn("No se encontraron platos en la respuesta de la API.");
            return [];
        }
    } catch (error) {
        console.error("Error al obtener los platos:", error);
        return [];
    }
}


export async function Search(filters = {}) {
    try {
        const params = new URLSearchParams();
        
        // Desestructuramos los filtros para que sea más fácil de leer
        const { name, category, sortByPrice, onlyActive } = filters;

        // Añadimos cada parámetro a la URL SÓLO SI tiene un valor
        if (name) {
            params.append('name', name);
        }
        if (category) {
            params.append('category', category);
        }
        if (sortByPrice) {
            params.append('sortByPrice', sortByPrice);
        }
        // Ojo con los booleanos: hay que verificar que no sean undefined o null
        if (onlyActive !== undefined && onlyActive !== null) {
            params.append('onlyActive', onlyActive);
        }

        const url = `${API_BASE}?${params.toString()}`;
        console.log("Llamando a la API con filtros:", url);

        const response = await axios.get(url);
        
        if (response.data && Array.isArray(response.data)) {
            return response.data;
        } else {
            console.warn("La respuesta de la API no contiene una lista de platos.");
            return [];
        }
    } catch (error) {
        console.error("Error al obtener los platos:", error);
        return [];
    }
}

export async function GetDishById(id) {
    try {
        const response = await axios.get(`${API_BASE}/${id}`);
        return response.data;
    } catch (error) {
        console.error("Error al obtener el plato:", error);
        return null;
    }
}
export async function CreateDish(dish) {
    try {
        const response = await axios.post(API_BASE, dish);
        return response.data;
    } catch (error) {
        console.error("Error al crear el plato:", error);
        return null;
    }
}
export async function UpdateDish(id, dishUpdated) {
    try {
        const response = await axios.put(`${API_BASE}/${id}`, dishUpdated,{
            headers: {
                'Content-Type': 'application/json'
            }
        });
        return response.data;
    } catch (error) {
        console.error("Error al actualizar el plato:", error);
        return null;
    }
}
export async function deleteDish(id) {
    try {
        const response = await axios.delete(`${API_BASE}/${id}`);
        return response.data;
    } catch (error) {
        console.error("Error al eliminar el plato:", error);
        return null;
    }
}