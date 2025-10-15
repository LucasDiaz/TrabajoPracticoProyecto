// import { Search } from "../Apis/DishApi.js";

export async function loadAllDishesAndRender() {
    try {
        const platos = await Search(null, null, 'ASC', true);
        if (platos && platos.length > 0) {
            renderizarCards(platos);
        } else {
            console.log("El menú está vacío.");
        }
    } catch (error) {
        console.error("No se pudo cargar el menú completo.", error);
    }
}

