namespace Application.ROP
{
    /// <summary>
    /// Catálogo de errores de dominio
    /// </summary>
    public static class Errors
    {
        // General
        public static Error NOT_FOUND => new("404", "Recurso no encontrado");

        // Donas
        public static Error DONUT_NAME_NOT_AVAILABLE => new("DNT01", "Nombre de dona no disponible");
    }
}
