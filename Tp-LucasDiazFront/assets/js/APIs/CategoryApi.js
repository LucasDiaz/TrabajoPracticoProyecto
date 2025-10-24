const API_BASE_URL = 'https://localhost:7036/api/v1'; 

export async function getCategories() {
    try {
        const response = await fetch(`${API_BASE_URL}/Category`);
        if (!response.ok) throw new Error('Error al obtener categorías');
        return await response.json();
    } catch (error) {
        console.error("Error en getCategories:", error);
        return [];
    }
}