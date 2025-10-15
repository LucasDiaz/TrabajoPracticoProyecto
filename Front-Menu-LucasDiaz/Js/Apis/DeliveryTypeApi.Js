const API_BASE = "https://localhost:7036/api/v1/DeliveryTypes";



/**
 * Obtiene todas las categorías disponibles desde la API.
 * * @returns {Promise<Array<Object>|null>} Una promesa que resuelve con un array de categorías 
 * o null si ocurre un error.
 */
export async function GetAllDeliveryTypes() {
    // 1. Construir la URL completa del endpoint
    // Asumo que el endpoint es simplemente la URL base.


    try {
        // 2. Realizar la solicitud GET de forma asíncrona
        const response = await fetch(API_BASE, {
            method: 'GET',
            headers: {
                // Puedes añadir encabezados necesarios aquí, como tokens de autorización o Content-Type
                'Content-Type': 'application/json',
            },
        });

        // 3. Manejo de errores de respuesta HTTP (ej: 400 Bad Request, 404 Not Found)
        if (!response.ok) {
            // Lanza un error si el estado HTTP no es 2xx (Successful)
            console.error(`Error HTTP: ${response.status} - ${response.statusText}`);
            
            // Intenta leer el cuerpo del error (si existe)
            const errorBody = await response.json().catch(() => ({ message: 'Error desconocido del servidor' }));
            
            console.error("Detalles del Error:", errorBody);
            
            // Puedes lanzar el error para que sea capturado por el componente que llama
            throw new Error(errorBody.message || `La solicitud falló con estado ${response.status}`);
        }

        // 4. Procesar y devolver los datos JSON
        const data = await response.json();
        return data; // Devolverá el IEnumerable<DeliveryTypeResponse>
        
    } catch (error) {
        // 5. Manejo de errores de red (ej: el servidor no está disponible)
        console.error("Error al obtener los tipos de delivery:", error.message);
        
        // Devolvemos un array vacío o null, dependiendo de cómo quieras manejarlo en el frontend
        return null; 
    }
}