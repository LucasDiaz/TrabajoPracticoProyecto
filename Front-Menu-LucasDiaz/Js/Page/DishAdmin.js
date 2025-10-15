

import { Search as SearchDish } from '../Apis/DishApi.js';
import { renderCardDish } from './../Renders/renderCardsDish.js'; 
import {actualizarVistaCarrito} from './../Renders/renderCarrito.js';
import * as CartHandler from './../Handlers/OrderHandlers/OrderHandler.js';
import {configurarFlujoPedido} from './../Handlers/OrderHandlers/OrderFlowHandler.js';

// --- 2. Referencias del DOM y Estado ---
const contenedorDishes = document.getElementById('dishes-container');
const inputBusqueda = document.getElementById('input-busqueda');
// const categoryFiltersContainer = document.getElementById('category-filters-container');
const sortByPriceSelect = document.getElementById('sort-by-price');
const btnBuscar = document.getElementById('btn-buscar');
const onlyActiveCheckbox = document.getElementById('only-active-checkbox');

// Objeto para guardar el estado actual de los filtros de búsqueda
const currentFilters = {
    name: '',
    // category: null,
    sortByPrice: 'ASC', // Valor por defecto
    onlyActive: null
};

let debounceTimeout;


// --- 3. Funciones de Renderizado y API ---

/**
 * Función que se encarga de renderizar la lista de platos en el contenedor.
 * @param {Array} dishList - La lista de platos a mostrar.
 */
function DishRender(dishList) {
    // if (!contenedorDishes) return;
    contenedorDishes.innerHTML = ''; // Limpiamos el contenedor

    if (dishList.length === 0) {
        contenedorDishes.innerHTML = '<p class="text-center text-muted">No se encontraron platos.</p>';
        return;
    }
    const fragment = document.createDocumentFragment();
    dishList.forEach(dish => {
        const tarjetaElemento = renderCardDish(dish);
        fragment.appendChild(tarjetaElemento);
    });
    contenedorDishes.appendChild(fragment);
}

async function applyFiltersAndRender() {
    
    contenedorDishes.innerHTML = '<p class="text-center">Buscando...</p>';
    try {
        // Llama a la API con los filtros actuales
        const dishes = await SearchDish(currentFilters); 
        console.log("Platos obtenidos:", dishes);
        DishRender(dishes);
        
    } catch (error) {
        console.error("Error al aplicar filtros:", error);
        if (contenedorDishes) {
            contenedorDishes.innerHTML = '<p class="text-center text-danger">Error al cargar la lista de platos.</p>';
        }
    }
}


export async function inicializarPaginaDishAdmin() {
    console.log("Inicializando página de administración de platos...");
    await applyFiltersAndRender(); 
    
    // Configuramos el listener para la búsqueda por nombre
    inputBusqueda.addEventListener('input', () => {
        clearTimeout(debounceTimeout);
        debounceTimeout = setTimeout(() => {
            // ---> PASO 3: Actualizamos el estado de los filtros y volvemos a renderizar
            currentFilters.name = inputBusqueda.value.trim();
            applyFiltersAndRender();
        }, 300);
    });
    actualizarVistaCarrito(CartHandler.getCarrito());
    await configurarFlujoPedido(); 
    
   
    //filtro por precio
    sortByPriceSelect.addEventListener('change', () => {
        currentFilters.sortByPrice = sortByPriceSelect.value;
        applyFiltersAndRender();
    });
    
    
    //order 
   if (contenedorDishes) {
        contenedorDishes.addEventListener('click', (event) => {
             // ... (tu lógica de agregar al carrito) ...
             const botonAgregar = event.target.closest('.btn-agregar-pedido');
             if (botonAgregar) {
                const dishData = { 
                    id: botonAgregar.dataset.dishId,
                    name: botonAgregar.dataset.dishName,
                    price: parseFloat(botonAgregar.dataset.dishPrice),
                    imageUrl: botonAgregar.dataset.dishImageUrl,
                };
                CartHandler.agregarAlCarrito(dishData);
                actualizarVistaCarrito(CartHandler.getCarrito()); 
             }
        });
    }

    const carritoModalBody = document.getElementById('carrito-items-container');

    if (carritoModalBody) {
        carritoModalBody.addEventListener('click', (event) => {
            const target = event.target;
            
            // Usamos .closest para encontrar el elemento que tiene el data-dish-id
            // Esto funciona aunque hagas clic en el ícono o en el borde del botón
            const dishId = target.closest('[data-dish-id]')?.dataset.dishId; 
            
            if (!dishId) return;

            // 1. Botón Incrementar (+)
            if (target.classList.contains('btn-incrementar')) {
                CartHandler.modificarCantidad(dishId, 1);
            }
            // 2. Botón Decrementar (-)
            else if (target.classList.contains('btn-decrementar')) {
                CartHandler.modificarCantidad(dishId, -1);
            }
            // 3. Botón Eliminar (Papelera)
            else if (target.closest('.btn-eliminar')) {
                CartHandler.eliminarDelCarrito(dishId);
            }
            
            // 4. Después de cualquier modificación, actualizamos la vista del Modal
            actualizarVistaCarrito(CartHandler.getCarrito());
        });
    }






}

