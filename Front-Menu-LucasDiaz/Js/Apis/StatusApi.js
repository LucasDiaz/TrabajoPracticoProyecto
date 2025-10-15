const API_BASE_URL = 'https://localhost:7036/api/v1/Status';
/**
 * Función genérica para manejar respuestas HTTP, extrayendo datos o lanzando errores.
 * @param {Response} response Objeto de respuesta de la función fetch.
 * @returns {Promise<Object>} Promesa que resuelve con los datos JSON.
 */
async function handleResponse(response) {
    const isJson = response.headers.get('content-type')?.includes('application/json');
    const data = isJson ? await response.json() : null;

    if (!response.ok) {
        // Lanzar un error con el mensaje de la API o un mensaje genérico
        const error = (data && data.message) || response.statusText;
        console.error(`Error HTTP ${response.status}:`, data || response.statusText);
        throw new Error(error);
    }

    return data;
}

/**
 * Obtiene todos los estados de órdenes disponibles desde la API.
 * Corresponde al [HttpGet] del backend.
 * * @returns {Promise<Array<Object>|null>} Una promesa que resuelve con un array de estados 
 * o null si ocurre un error de red.
 */
export async function GetAllStatuses() {
    const url = API_BASE_URL;

    try {
        // Realizar la solicitud GET
        const response = await fetch(url, { method: 'GET' });

        // Manejar el resultado (200 OK, 400 Bad Request, etc.)
        const statuses = await handleResponse(response);
        
        return statuses; // Devolverá el IEnumerable<StatusResponse>
        
    } catch (error) {
        // Manejo de errores de red o errores lanzados por handleResponse
        console.error("Error al obtener los estados:", error.message);
        
        // Devolvemos un valor seguro en caso de fallo
        return null; 
    }
}