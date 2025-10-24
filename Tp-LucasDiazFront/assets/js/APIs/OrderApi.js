const API_BASE_URL = 'https://localhost:7036/api/v1'; 


export async function createOrder(orderRequest) {
    try {
        const response = await fetch(`${API_BASE_URL}/Order`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(orderRequest),
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Error al crear la orden.');
        }
        return await response.json();
    } catch (error) {
        console.error("Error en createOrder:", error);
        return { error: error.message };
    }
}


export async function getOrders(filters = {}) {
    try {
        const url = new URL(`${API_BASE_URL}/Order`);

       
        if (filters.from) {
            url.searchParams.append('from', filters.from);
        }
        if (filters.to) {
            url.searchParams.append('to', filters.to);
        }
        
        const response = await fetch(url);
        if (!response.ok) throw new Error('Error al obtener las órdenes');
        return await response.json();
    } catch (error) {
        console.error("Error en getOrders:", error);
        return { error: error.message };
    }
}

export async function updateOrderItemStatus(orderId, itemId, newStatus) {
    try {
        const response = await fetch(`${API_BASE_URL}/Order/${orderId}/item/${itemId}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ status: newStatus }),
        });
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Error al actualizar el estado.');
        }
        return await response.json();
    } catch (error) {
        console.error("Error en updateOrderItemStatus:", error);
        return { error: error.message };
    }
}

export async function getOrderById(orderId) {
    try {
        const response = await fetch(`${API_BASE_URL}/Order/${orderId}`);
        if (!response.ok) throw new Error('Orden no encontrada');
        return await response.json();
    } catch (error) {
        console.error(`Error al obtener orden ${orderId}:`, error);
        return null; 
    }
}


export async function updateOrder(orderId, updateRequest) {
    try {
        const response = await fetch(`${API_BASE_URL}/Order/${orderId}`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updateRequest),
        });

       
        if (!response.ok) {
            
            const errorText = await response.text();
            
            throw new Error(errorText || response.statusText);
        }

        
        const contentLength = response.headers.get('content-length');
        if (!contentLength || contentLength === '0') {
            return { success: true }; 
        }
        
        
        return await response.json();

    } catch (error) {
        console.error("Error en updateOrder:", error);
        return { error: error.message };
    }
}

export async function updateOrderStatus(orderId, newStatusId) {
    try {
        const response = await fetch(`${API_BASE_URL}/Order/${orderId}/status`, { 
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ status: newStatusId }),
        });
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'Error al actualizar el estado de la orden.');
        }
        return { success: true };
    } catch (error) {
        console.error("Error en updateOrderStatus:", error);
        return { error: error.message };
    }
}
