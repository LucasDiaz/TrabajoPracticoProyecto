const API_BASE_URL = 'https://localhost:7036/api/v1'; 


export async function getDeliveryTypes() {
    try {
        const response = await fetch(`${API_BASE_URL}/DeliveryType`);
        if (!response.ok) throw new Error('Error al obtener tipos de entrega');
        return await response.json();
    } catch (error) {
        console.error("Error en getDeliveryTypes:", error);
        return [];
    }
}