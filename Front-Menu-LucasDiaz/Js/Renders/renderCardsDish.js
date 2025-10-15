// En: Assets/JS/Render/render.js (o la ruta de tu componente)

/**
 * Crea y devuelve el elemento DOM (tarjeta de Bootstrap) para un plato dado.
 * @param {Object} dish - Objeto del plato (DishResponse).
 * @returns {HTMLElement} El elemento div que contiene la Card de Bootstrap.
 */
export function renderCardDish(dish) { 
    // 1. Creamos el DIV que servirá como columna en la grilla de Bootstrap
    const wrapper = document.createElement("div");
    // Usamos las clases de Grid para que se muestren 3 o 4 por fila
    wrapper.classList.add("col-lg-3", "col-md-4", "mb-4"); 

    // 2. Creamos el contenido de la card (Usando Template Literals: backticks ` `)
    // También nos aseguramos de que el precio sea flotante para el formato.
    const formattedPrice = parseFloat(dish.price).toFixed(2); 

    wrapper.innerHTML = ` 
        <div class="card h-100 text-center shadow">
            <img src="${dish.image}" 
                 class="card-img-top" 
                 alt="${dish.name}"
                 style="height: 180px; object-fit: cover;">
                 
            <div class="card-body d-flex flex-column">
                <h5 class="card-title">${dish.name}</h5>
                <p class="card-text text-muted">${dish.description || 'Plato delicioso.'}</p>
            </div>
            
            <div class="card-footer">
                <p class="h4 m-0 mb-3 text-success">$${formattedPrice}</p>
                
                <button class="btn btn-primary btn-agregar-pedido w-100"  
                            data-dish-id="${dish.id}" 
                            data-dish-name="${dish.name}" 
                            data-dish-price="${formattedPrice}" 
                            data-dish-image-url="${dish.imageUrl}"> 
                        <i class="bi bi-cart-plus"></i> Agregar
                </button>
            </div>
        </div>
    `; // Cierre con backtick

    // 3. Devolvemos el elemento completo (columna + tarjeta)
    return wrapper;
}