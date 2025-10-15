// JS/Handlers/CarritoHandler.js

// Recupera la orden del almacenamiento local o inicializa un array vacío
let carritoItems = /*JSON.parse(localStorage.getItem('currentOrder')) || */ [];

/**
 * Agrega un plato al carrito o incrementa su cantidad.
 * @param {Object} dishData - Objeto del plato (id, name, price, imageUrl, etc.).
 */
export function agregarAlCarrito(dishData) {
    const itemIndex = carritoItems.findIndex(item => item.id === dishData.id);

    if (itemIndex > -1) {
        // Si ya existe, incrementa la cantidad
        carritoItems[itemIndex].cantidad += 1;
    } else {
        // Si es nuevo, lo agrega con cantidad inicial 1
        carritoItems.push({
            ...dishData,
            cantidad: 1
        });
    }
    // Sincroniza el estado con el navegador
    localStorage.setItem('currentOrder', JSON.stringify(carritoItems));
}

/**
 * Modifica la cantidad de un plato existente por un incremento/decremento.
 */
export function modificarCantidad(dishId, cambio) {
    const itemIndex = carritoItems.findIndex(item => item.id === dishId);
    if (itemIndex > -1) {
        carritoItems[itemIndex].cantidad += cambio;
        // Elimina el item si la cantidad llega a cero o menos
        if (carritoItems[itemIndex].cantidad <= 0) {
            eliminarDelCarrito(dishId);
        } else {
            localStorage.setItem('currentOrder', JSON.stringify(carritoItems));
        }
    }
}

/**
 * Elimina completamente un plato del carrito.
 */
export function eliminarDelCarrito(dishId) {
    carritoItems = carritoItems.filter(item => item.id !== dishId);
    localStorage.setItem('currentOrder', JSON.stringify(carritoItems));
}

/**
 * @returns {Array} La lista de items en la orden actual.
 */
export function getCarrito() {
    return carritoItems;
}


export function clearCarrito() {
    carritoItems = []; // Vacía el array en memoria
    localStorage.removeItem('currentOrder'); // Elimina la entrada del navegador
    // No necesita devolver nada, solo actualiza el estado.
}
