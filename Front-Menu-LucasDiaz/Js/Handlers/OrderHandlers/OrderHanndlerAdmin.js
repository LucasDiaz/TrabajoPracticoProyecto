// orderHandler.js
import { GetOrders } from '../../Apis/OrdenApi.js'; // Usa 'getOrders' (minúscula) como en tu modificación
import { createOrderCard } from '../../Renders/cardOrder.js';

const mainContent = document.querySelector('.main-content-custom');

/**
 * Carga las órdenes de la API y las renderiza en el contenedor principal.
 * @param {Object} filters - Filtros opcionales (statusId, from, to).
 */
export async function loadOrders(filters = {}) {
    mainContent.innerHTML = '<div class="text-center text-white py-5">Cargando órdenes...</div>'; // Estado de carga

    try {
        // La llamada a la API con la nueva función getOrders basada en Axios
        const orders = await GetOrders(filters); 

        if (!orders || orders.length === 0) {
            mainContent.innerHTML = '<div class="text-center text-white py-5">No hay órdenes para mostrar.</div>';
            return;
        }

        // 1. Crear el contenedor Grid (row)
        const gridContainer = document.createElement('div');
        gridContainer.className = 'row p-4'; 

        // 2. Generar el HTML de las tarjetas
        const cardsHtml = orders.map(order => createOrderCard(order)).join('');
        
        // 3. Renderizar
        gridContainer.innerHTML = cardsHtml;
        mainContent.innerHTML = '';
        mainContent.appendChild(gridContainer);
        
        // Disparar evento para que OrderAdmin pueda enlazar listeners a las nuevas tarjetas
        const event = new CustomEvent('ordersLoaded');
        document.dispatchEvent(event);

    } catch (error) {
        // La función getOrders ya maneja y loguea el error, aquí solo mostramos un mensaje genérico
        mainContent.innerHTML = `<div class="alert alert-danger mx-auto mt-4" role="alert">
            Error al cargar las órdenes. Por favor, revisa la consola para más detalles.
        </div>`;
    }
}