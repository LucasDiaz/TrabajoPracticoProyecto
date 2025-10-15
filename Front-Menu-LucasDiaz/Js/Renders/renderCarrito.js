// JS/Components/Dishes/renderCarritoItem.js

const contenedorItems = document.getElementById('carrito-items-container');
const botonConfirmar = document.querySelector('#carritoModal .btn-success'); // Botón de Confirmar Pedido

/**
 * Renderiza la lista de ítems y el total en el cuerpo del modal.
 * @param {Array} carrito - Lista de ítems del CarritoHandler.
 */
export function actualizarVistaCarrito(carrito) {
    if (!contenedorItems) return;
    
    if (carrito.length === 0) {
        contenedorItems.innerHTML = '<p class="text-center text-muted">Tu carrito está vacío.</p>';
        botonConfirmar.disabled = true;
        return;
    }
    
    let htmlContent = '';
    let total = 0;

    carrito.forEach(item => {
        const subtotal = item.price * item.cantidad;
        total += subtotal;

        htmlContent += `
            <div class="d-flex align-items-center border-bottom py-2" data-dish-id="${item.id}">
                <img src="${item.image}" style="width: 60px; height: 60px; object-fit: cover; margin-right: 15px;" class="rounded">
                <div class="flex-grow-1">
                    <h6 class="mb-0">${item.name}</h6>
                    <small class="text-muted">Precio: $${item.price.toFixed(2)}</small>
                </div>
                <div class="d-flex align-items-center">
                    <button class="btn btn-sm btn-outline-danger btn-decrementar me-2" data-dish-id="${item.id}">-</button>
                    <span class="fw-bold me-2">${item.cantidad}</span>
                    <button class="btn btn-sm btn-outline-success btn-incrementar" data-dish-id="${item.id}">+</button>
                </div>
                <div class="ms-4 fw-bold text-end" style="width: 80px;">
                    $${subtotal.toFixed(2)}
                </div>
                <button class="btn btn-sm btn-link text-danger ms-2 btn-eliminar" data-dish-id="${item.id}">
                    <i class="bi bi-trash"></i>
                </button>
            </div>
        `;
    });

    // Añadir el total al final
    htmlContent += `
        <div class="d-flex justify-content-end pt-3">
            <h4 class="me-3">Total:</h4>
            <h4 class="text-primary">$${total.toFixed(2)}</h4>
        </div>
    `;

    contenedorItems.innerHTML = htmlContent;
    botonConfirmar.disabled = false;
}