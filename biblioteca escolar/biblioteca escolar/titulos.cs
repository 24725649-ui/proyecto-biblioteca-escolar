using System;

namespace BibliotecaEscolar
{
    // Representa un ejemplar físico individual
    public class Ejemplar
    {
        public int IdEjemplar { get; set; }
        public bool Disponible { get; set; }

        public Ejemplar(int idEjemplar, bool disponible = true)
        {
            IdEjemplar = idEjemplar;
            Disponible = disponible;
        }
    }

    // Representa una obra/título en el catálogo
    public class Titulo
    {
        public string ISBN { get; set; }
        public string Nombre { get; set; }
        public int PrestamosHistoricos { get; set; }

        // Arreglo con la cantidad exacta de ejemplares (sin celdas vacías)
        public Ejemplar[] Ejemplares { get; set; }

        public Titulo(string isbn, string nombre, int prestamosHistoricos, int cantidadCopias)
        {
            ISBN = isbn;
            Nombre = nombre;
            PrestamosHistoricos = prestamosHistoricos;

            // Se reserva memoria ÚNICAMENTE para la cantidad exacta requerida
            Ejemplares = new Ejemplar[cantidadCopias];
            for (int i = 0; i < cantidadCopias; i++)
            {
                Ejemplares[i] = new Ejemplar(i + 1, true);
            }
        }

        // Método auxiliar para obtener disponibles sin celdas nulas
        public int ObtenerDisponibles()
        {
            int disponibles = 0;
            foreach (var ej in Ejemplares)
            {
                if (ej.Disponible) disponibles++;
            }
            return disponibles;
        }
        // 2. Registro de préstamo: reduce disponibilidad e incrementa préstamos históricos
        public bool RegistrarPrestamo()
        {
            // Buscar el primer ejemplar disponible
            foreach (var ej in Ejemplares)
            {
                if (ej.Disponible)
                {
                    ej.Disponible = false;      // Se marca como prestado
                    PrestamosHistoricos++;      // Se incrementa el contador histórico
                    return true;                // Préstamo exitoso
                }
            }

            // Si no hay ejemplares disponibles
            return false;
        }
    }
}
    
