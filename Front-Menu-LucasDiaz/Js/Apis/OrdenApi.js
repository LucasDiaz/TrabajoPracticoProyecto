const API_BASE = "https://localhost:7036/api/v1/Order";

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
export async function CreateOrder(orderRequest) {
    const url = API_BASE;

    try {
         const response = await axios.post(API_BASE, orderRequest, {
            headers: { "Content-Type": "application/json" },
        });
         return response.data;
        
        } 
        catch (error) {
        console.error("Error al crear la orden:", error.message);
        throw error;
    }
}


/**
 * Obtiene una lista de órdenes con filtros opcionales. Corresponde al [HttpGet] del backend.
 * @param {number | null} statusId Filtro opcional por ID de estado.
 * @param {string | null} from Fecha de inicio opcional (formato YYYY-MM-DD o similar).
 * @param {string | null} to Fecha de fin opcional (formato YYYY-MM-DD o similar).
 * @returns {Promise<Array<Object>>} Promesa que resuelve con IEnumerable<OrderDetailsResponse> (HTTP 200).
 */
export async function GetOrders(statusId = null, from = null, to = null) {
    
    // Construir los Query Parameters
    const params = new URLSearchParams();
    if (statusId !== null) params.append('statusId', statusId);
    if (from !== null) params.append('from', from);
    if (to !== null) params.append('to', to);

    const url = `${API_BASE_URL}?${params.toString()}`;

    try {
        const response = await fetch(url, { method: 'GET' });
        
        // Maneja HTTP 200 OK y posibles errores (400, 404)
        return await handleResponse(response);
        
    } catch (error) {
        console.error("Error al obtener las órdenes filtradas:", error.message);
        throw error;
    }
}

export async function GetOrderById(id) {
    const url = `${API_BASE_URL}/${id}`;

    try {
        const response = await fetch(url, { method: 'GET' });
        
        // Maneja HTTP 200 OK y posibles errores (404)
        return await handleResponse(response);
        
    } catch (error) {
        console.error(`Error al obtener la orden con ID ${id}:`, error.message);
        throw error;
    }
}
/**
 * Actualiza la cantidad de items en una orden. Corresponde al [HttpPatch("{orderId}")] del backend.
 * @param {number} orderId El ID de la orden a modificar.
 * @param {Object} request El cuerpo de la solicitud (OrderUpdateRequest).
 * @returns {Promise<Object>} Promesa que resuelve con OrderUpdateReponse (HTTP 200).
 */
export async function UpdateOrderItems(orderId, request) {
    const url = `${API_BASE_URL}/${orderId}`;

    try {
        const response = await fetch(url, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        });
        
        // Maneja HTTP 200 OK y posibles errores (404, 400)
        return await handleResponse(response);
        
    } catch (error) {
        console.error(`Error al actualizar items de la orden ${orderId}:`, error.message);
        throw error;
    }
}
/**
 * Actualiza el estado de un item específico dentro de una orden. Corresponde al [HttpPatch("{orderId}/item/{itemId}")] del backend.
 * @param {number} orderId El ID de la orden.
 * @param {number} itemId El ID del item dentro de la orden.
 * @param {Object} request El cuerpo de la solicitud (OrderItemUpdateRequest) con el nuevo estado.
 * @returns {Promise<Object>} Promesa que resuelve con OrderUpdateReponse (HTTP 200).
 */
export async function UpdateOrderItemStatus(orderId, itemId, request) {
    const url = `${API_BASE_URL}/${orderId}/item/${itemId}`;

    try {
        const response = await fetch(url, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        });
        
        // Maneja HTTP 200 OK y posibles errores (404, 400)
        return await handleResponse(response);
        
    } catch (error) {
        console.error(`Error al actualizar estado del item ${itemId} en la orden ${orderId}:`, error.message);
        throw error;
    }
}