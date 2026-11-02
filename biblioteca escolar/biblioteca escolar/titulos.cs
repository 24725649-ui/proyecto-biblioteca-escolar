using System;

namespace BibliotecaEscolar
{
   
    // CLASE: Ejemplar
    
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

    
    // CLASE: Titulo
    
    public class Titulo
    {
        public string ISBN { get; set; }
        public string Nombre { get; set; }
        public int PrestamosHistoricos { get; set; }

        // Arreglo dimensionado exactamente a la cantidad de copias
        public Ejemplar[] Ejemplares { get; set; }

        // Constructor principal 
        public Titulo(string isbn, string nombre, int prestamosHistoricos, int cantidadCopias)
        {
            ISBN = isbn;
            Nombre = nombre;
            PrestamosHistoricos = prestamosHistoricos;

            // Reserva de memoria exacta según la cantidad de ejemplares requerida
            Ejemplares = new Ejemplar[cantidadCopias];
            for (int i = 0; i < cantidadCopias; i++)
            {
                Ejemplares[i] = new Ejemplar(i + 1, true);
            }
        }

       

        // 1. Devuelve cuántos ejemplares están disponibles para préstamo actualmente
        public int ObtenerDisponibles()
        {
            int disponibles = 0;
            foreach (var ej in Ejemplares)
            {
                if (ej.Disponible) disponibles++;
            }
            return disponibles;
        }

        // 2. Registra un préstamo: marca un ejemplar disponible como prestado e incrementa el contador histórico
        public bool RegistrarPrestamo()
        {
            foreach (var ej in Ejemplares)
            {
                if (ej.Disponible)
                {
                    ej.Disponible = false;      // Cambia estado del ejemplar
                    PrestamosHistoricos++;      // Incrementa historial de préstamos
                    return true;                // Éxito en la operación
                }
            }

            // Si no quedan ejemplares libres
            return false;
        }

       

        // 3. Evalúa si este objeto debe ir ANTES que otro en el catálogo ordenado por préstamos:
        // Criterio 1: Mayor cantidad de préstamos históricos (orden descendente).
        // Criterio 2: Si hay empate en préstamos, ordena alfabéticamente (A-Z) para garantizar estabilidad.
        public bool DebeIrAntesQue(Titulo otro)
        {
            if (this.PrestamosHistoricos > otro.PrestamosHistoricos)
            {
                return true;
            }
            else if (this.PrestamosHistoricos == otro.PrestamosHistoricos)
            {
                return string.Compare(this.Nombre, otro.Nombre, StringComparison.OrdinalIgnoreCase) < 0;
            }

            return false;
        }
    }
}