using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Exceptions
{
    public abstract class ApplicationValidationException : Exception
    {
        public ApplicationValidationException(string message) : base(message) { }
    }

    // Excepción para el plato no válido (invalid_dish)
    public class InvalidDishException : ApplicationValidationException
    {
        public InvalidDishException()
            : base("El plato especificado no existe o no está disponible") { }

        public InvalidDishException(Guid dishId)
            : base($"El plato con ID '{dishId}' no existe o no está disponible") { }
    }

    // Excepción para la cantidad inválida (invalid_quantity)
    public class InvalidQuantityException : ApplicationValidationException
    {
        public InvalidQuantityException()
            : base("La cantidad debe ser mayor a 0") { }

        public InvalidQuantityException(Guid DishId)
            : base($"La cantidad debe ser mayor a 0 para el plato con ID '{DishId}'") { }
    }

    // Excepción para el tipo de entrega faltante (missing_delivery)
    public class MissingDeliveryTypeException : ApplicationValidationException
    {
        public MissingDeliveryTypeException()
            : base("Debe especificar un tipo de entrega válido") { }
    }

    // Excepción para listas vacias
    public class InvalidOperationException : ApplicationValidationException
    {
        public InvalidOperationException(string message)
            : base(message) { }
    }
    // Excepción para 404 Not Found (Orden no encontrada)
    public class OrderNotFoundException : ApplicationValidationException
    {
        public OrderNotFoundException(long orderId)
            : base($"Orden con ID {orderId} no encontrada") { }
    }

    // Excepción para 404 Not Found (OrdenItem no encontrada)
    public class OrderItemNotFoundException : ApplicationValidationException
    {
        public OrderItemNotFoundException(long orderId, int itemId)
            : base($"Item no encontrado (ID: {itemId}) en la orden {orderId}") { }
    }

    // Excepción para 400 Bad Request (Orden en estado no modificable)
    public class InvalidOrderStatusException : ApplicationValidationException
    {
        public InvalidOrderStatusException(string currentStatus)
            : base($"La orden está en estado '{currentStatus}' y no se puede modificar.") { }
    }

    // Excepción para 400 Bad Request (status no existe)
    public class InvalidStatusException : ApplicationValidationException
    {
        public InvalidStatusException(int idstatus)
            : base($"No existe el status '{idstatus}'") { }
    }

    // Excepción para 400 Bad Request (Plato no válido/no disponible en el recálculo)
    public class ItemRecalculationException : ApplicationValidationException
    {
        public ItemRecalculationException(Guid dishId)
            : base($"El plato con ID '{dishId}' no existe o no está disponible para el recálculo.") { }
    }
}
