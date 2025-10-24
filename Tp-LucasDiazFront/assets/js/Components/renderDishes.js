// cardDish.js

function getDishImageUrl(imageUrl) {
    if (imageUrl && imageUrl.trim() !== '') {
        return imageUrl;
    }
    // URL corregida y 100% válida
    return 'https://i.ibb.co/Gv0zcJQW/Sin-t-tulo-Folleto-A4-con-doblez-en-el-medio-420-mm-x-297-mm.png';
}

/**
 * Renderiza la lista de platos en el contenedor principal.
 * @param {Array} dishes - La lista de platos a mostrar.
 */
export function renderDishes(dishes) {
    const container = document.getElementById('dish-list-container');
    if (!container) return;

    if (dishes.length === 0) {
        container.innerHTML = '<p class="text-muted text-center col-12">No se encontraron platos que coincidan con la búsqueda.</p>';
        return;
    }

    container.innerHTML = dishes.map(dish => `
        <div class="col-12 col-md-6 col-lg-4 mb-4">
            <div class="card h-100 dish-card shadow-sm">
                <div class="row g-0">
                    <div class="col-auto">
                        <img src="${getDishImageUrl(dish.image)}" class="img-fluid rounded-start dish-card-img" alt="${dish.name}" style="width: 150px; height: 150px; object-fit: cover;">
                    </div>
                    <div class="col">
                        <div class="card-body d-flex flex-column h-100 p-3">
                            <h5 class="dish-name mb-1">${dish.name}</h5>
                            <p class="dish-price fs-5 fw-bold text-success mt-1">$${dish.price.toFixed(2)}</p>
                            
                            <div class="mt-auto">
                                <button class="btn btn-primary w-100 add-to-cart-btn" data-dish-id="${dish.id}">
                                    Agregar a la Comanda
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `).join('');
}