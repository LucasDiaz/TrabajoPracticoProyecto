// orderAdmin.js
import { loadOrders } from '../Handlers/OrderHandlers/OrderAdminHandler.js';

/**
 * Función para asignar todos los manejadores de eventos (listeners) necesarios.
 */
function attachEventListeners() {
    console.log("EventListeners de OrderAdmin adjuntos. Listo para gestionar filtros y acciones.");
    
    // Aquí puedes añadir listeners para botones de acción o filtros.
    // Ejemplo de cómo manejar acciones en las tarjetas después de que se cargan:
    /*
    document.addEventListener('ordersLoaded', () => {
        // Lógica de listeners delegados en el contenedor si los necesitas
    });
    */
}

/**
 * Inicializa la página de administración de órdenes: carga datos y asigna listeners.
 */
export function inicializarPaginaOrderAdmin() {
    // 1. Cargar la vista inicial de órdenes (sin filtros)
    loadOrders();
    
    // 2. Adjuntar los listeners (ej: si tienes filtros de estado/fecha)
    attachEventListeners();
}