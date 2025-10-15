import { CreateOrder as createOrderApi } from '../../Apis/OrdenApi.js'; 
import * as CartHandler from './OrderHandler.js';
import { actualizarVistaCarrito } from '../../Renders/renderCarrito.js';

// Referencias a elementos UI clave
const carritoModalEl = document.getElementById('carritoModal');
const deliverySelect = document.getElementById('delivery-select');
const notesInput = document.getElementById('order-description'); 
const btnConfirmar = document.getElementById('btn-confirmar-orden');

// Instancias de Bootstrap Modal (se inicializan en el DOMContentLoaded)
let carritoModalInstance;
let deliveryModalInstance;

export function configurarFlujoPedido() {
    
    // Verificación de los elementos clave del DOM
    if (!btnConfirmar || !deliverySelect || !notesInput) {
        console.error("Error: Elementos de confirmación (Botón, Select o Notas) no encontrados en el DOM.");
        return;
    }    
    // --- LISTENER DEL BOTÓN "CONFIRMAR ORDEN" ---
    btnConfirmar.addEventListener('click', async () => {   
        // 1. Recolección de Datos
        const items = CartHandler.getCarrito();
        const selectedDeliveryId = deliverySelect.value;
        const orderNotes = notesInput.value.trim();
        
        // 2. Validación
        if (items.length === 0) {
            alert("El carrito está vacío. Agrega platos para continuar.");
            return;
        }
        if (!selectedDeliveryId) {
            alert("Por favor, selecciona un tipo de delivery.");
            return;
        }
        
      
    // 1. TRANSFORMACIÓN CRÍTICA: Crear la lista de ItemRequest
    const orderItemsRequest = items.map(item => ({
        // ¡Usamos los nombres de propiedades EXACTOS que espera el backend (ItemRequest)!
        Id: item.id,            // Tu DishId del carrito se mapea al ItemRequest.Id (Guid)
        quantity: item.cantidad, // Tu item.cantidad se mapea al ItemRequest.quantity
        notes: "",               // Puedes añadir notas por ítem si las tuvieras, sino, String vacío
    }));
    

    
    
    // 2. Construcción de DeliveryRequest
    const deliveryRequest = {
        id: parseInt(selectedDeliveryId), 
        to: "", // Usamos las notas como 'to' si no tienes otro campo
    };
    if (selectedDeliveryId === 1) { // 1 = Delivery
            deliveryRequest.to = "Delivery";
        } else if (deliveryId === 2) { // 2 = Retiro en local
            deliveryRequest.to = "Takeaway";;
        } else if (deliveryId === 3) { // 3 = Comida en el local
           deliveryRequest.to = "Dine in";;
        }
    
    // 3. OrderRequest Final (Usando las propiedades correctas)
    const orderRequest = {
        Items: orderItemsRequest, // <-- ARRAY TRANSFORMADO CORRECTAMENTE
        Delivery: deliveryRequest,
        Notes: orderNotes,
    };
        // 4. LLAMADA A LA API (createOrder)
        try {
            const response = await createOrderApi(orderRequest);

            // 5. Manejo de Éxito y Limpieza
            alert(`¡Pedido #${response.id} confirmado!`);
            
            CartHandler.clearCarrito(); 
            actualizarVistaCarrito(CartHandler.getCarrito()); 
            
            // Cerrar el Modal del Carrito
            const modalInstance = bootstrap.Modal.getInstance(carritoModalEl);
            if (modalInstance) {
                modalInstance.hide();
            }

        } catch (error) {
            console.error("Fallo al confirmar el pedido:", error);
            alert(`Error al confirmar el pedido: ${error.message || 'Error de conexión.'}`);
        }
    });

}