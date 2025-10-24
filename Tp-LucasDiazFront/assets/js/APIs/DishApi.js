const API_BASE_URL = 'https://localhost:7036/api/v1'; 


export async function getDishes(filters = {}) {
    try {
        const url = new URL(`${API_BASE_URL}/Dish`);
        
        
        Object.keys(filters).forEach(key => {
            if (filters[key]) {
                url.searchParams.append(key, filters[key]);
            }
        });
        
        const response = await fetch(url);
        if (!response.ok) throw new Error('Error al obtener los platos.');
        return await response.json();
    } catch (error) {
        console.error("Error en getDishes:", error);
        return [];
    }
}


export async function createDish(dishRequest) {
    try {
        const response = await fetch(`${API_BASE_URL}/Dish`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(dishRequest),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ocurrió un error al crear el plato.');
        }
        return await response.json();
    } catch (error) {
        console.error("Error en createDish:", error);
        return { error: error.message };
    }
}


export async function updateDish(dishId, dishUpdateRequest) {
    try {
        const response = await fetch(`${API_BASE_URL}/Dish/${dishId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(dishUpdateRequest),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Ocurrió un error al actualizar el plato.');
        }
        return await response.json();
    } catch (error) {
        console.error("Error en updateDish:", error);
        return { error: error.message };
    }
}


export async function deleteDish(dishId) {
    try {
       
        const response = await fetch(`${API_BASE_URL}/Dish/${dishId}`, {
            method: 'DELETE',
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Error al desactivar el plato.');
        }
        
        
        if (response.status === 204) {
            return { success: true };
        }
        
        return await response.json();
    } catch (error) {
        console.error("Error en deleteDish:", error);
        return { error: error.message };
    }
}