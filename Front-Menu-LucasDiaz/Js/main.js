
import { inicializarPaginaDishAdmin } from './Page/DishAdmin.js';

import{inicializarPaginaOrderAdmin} from './Page/OrderAdmin.js'

document.addEventListener('DOMContentLoaded', () => {
    const rutaActual = window.location.pathname;
    
        inicializarPaginaDishAdmin();

        inicializarPaginaOrderAdmin();
    
});
