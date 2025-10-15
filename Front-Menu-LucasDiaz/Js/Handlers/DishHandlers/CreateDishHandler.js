// CreateDishHandler.js
// Asumo que esta función se llama desde el router: ConfigurationCreateDish(apiClient.Dish.create, cargarPlatos)

/**
 * Configura el formulario de creación de plato en el Modal.
 * @param {function} createDishFn Función que llama a la API (apiClient.Dish.create).
 * @param {function} callbackSuccess Función que se ejecuta tras el éxito (DishAdmin.cargarPlatos).
 */
export function ConfigurationCreateDish(createDishFn, callbackSuccess) {
    
    const formAgregar = document.getElementById('formAgregar');
    // ... otros elementos UI ...

    if (!formAgregar) {
        return;
    }

    formAgregar.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        // ... (Tu lógica para recolectar y validar el objeto Dish) ...
        const Dish = { /* ... datos del formulario ... */ };

        try {
            await createDishFn(Dish); 
            
            // 1. Mostrar mensaje de éxito
            mostrarExito("Plato creado con éxito.");

            // 2. Ejecutar la función de recarga para actualizar la lista en el DOM
            callbackSuccess(); 

            // 3. Opcional: Cerrar el modal (se requiere la librería de Bootstrap Modal)
            const modalElement = document.getElementById('crearPlatoModal');
            if (modalElement) {
                 const modalBootstrap = bootstrap.Modal.getInstance(modalElement);
                 if(modalBootstrap) modalBootstrap.hide();
            }

        } catch (error) {
            mostrarError(error.message || "Error al crear plato");
        }
    });
}