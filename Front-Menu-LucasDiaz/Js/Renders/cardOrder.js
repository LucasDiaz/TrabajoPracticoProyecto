
// cardOrder.js

/**
 * Asigna una clase de color de Bootstrap al badge de estado.
 * @param {number} statusId - ID del estado de la orden.
 * @returns {string} Clase CSS de Bootstrap.
 */
function getStatusBadgeClass(statusId) {
    switch (statusId) {
        case 1: // Creada
            return 'bg-info text-dark';
        case 2: // En Preparación
            return 'bg-warning text-dark';
        case 3: // Lista para Entrega
            return 'bg-primary';
        case 4: // Entregada
            return 'bg-success';
        case 5: // Cancelada
            return 'bg-danger';
        default:
            return 'bg-secondary';
    }
}

/**
 * Genera el HTML para los detalles de los ítems de una orden.
 * @param {Array<Object>} items - Lista de ítems de la orden.
 * @returns {string} HTML de la lista de ítems.
 */
function createOrderItemList(items) {
    if (!items || items.length === 0) {
        return '<p class="text-muted fst-italic mb-0">Sin ítems</p>';
    }

    const displayItems = items.slice(0, 3);
    const remainingCount = items.length - displayItems.length;

    const listHtml = displayItems.map(item => `
        <li>
            <strong class="text-white">${item.Quantity}x</strong> ${item.dish.Name} 
            ${item.notes ? `<span class="text-secondary fst-italic">(${item.notes})</span>` : ''}
        </li>
    `).join('');

    const moreHtml = remainingCount > 0 
        ? `<li class="text-secondary small mt-1">+ ${remainingCount} ítem(s) más</li>`
        : '';

    return `<ul class="list-unstyled small mb-0 ps-3 text-light">${listHtml}${moreHtml}</ul>`;
}


/**
 * Crea una tarjeta (Card) de Bootstrap para mostrar la información de una orden.
 * @param {Object} order - Objeto de respuesta de la orden (OrderDetailsResponse).
 * @returns {string} El HTML completo de la tarjeta.
 */
export function createOrderCard(order) {
    // Aseguramos que createAt es un Date válido antes de formatear
    const date = order.createAt ? new Date(order.createAt) : null;
    const formattedDate = date ? date.toLocaleString('es-ES', { 
        day: '2-digit', month: 'short', hour: '2-digit', minute: '2-digit' 
    }) : 'Fecha Desconocida';

    const statusClass = getStatusBadgeClass(order.status.Id);
    const itemsHtml = createOrderItemList(order.items);

    return `
        <div class="col-12 col-md-6 col-lg-4 mb-4" data-order-id="${order.orderNumber}">
            <div class="card h-100 shadow-lg text-white" style="background-color: #495057;">
                
                <div class="card-header d-flex justify-content-between align-items-center" style="background-color: #343a40;">
                    <h5 class="mb-0 text-info">Orden #${order.orderNumber}</h5>
                    <span class="badge ${statusClass}">${order.status.Name}</span>
                </div>
                
                <div class="card-body">
                    <p class="card-text mb-1">
                        <strong class="text-warning">Total:</strong> 
                        <span class="fs-5">$${order.totalAmount.toFixed(2)}</span>
                    </p>
                    <p class="card-text small mb-1">
                        <strong class="text-light">Entrega:</strong> ${order.deliveryType.Name} en ${order.deliveryTo}
                    </p>

                    <h6 class="mt-3 text-white-50 border-bottom border-secondary pb-1">Productos:</h6>
                    ${itemsHtml}

                    ${order.notes ? `<p class="card-text mt-3 small fst-italic">Notas: ${order.notes}</p>` : ''}
                </div>
                
                <div class="card-footer text-end small text-muted" style="background-color: #343a40;">
                    Creada: ${formattedDate}
                </div>
            </div>
        </div>
    `;
}