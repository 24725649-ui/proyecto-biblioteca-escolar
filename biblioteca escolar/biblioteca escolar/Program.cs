using System;
using System.Collections.Generic;

namespace BibliotecaEscolar
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Carga inicial de los 8 títulos
            List<Titulo> catalogo = CargarCatalogoInicial();

            // 2. Verificación de carga y ausencia de celdas vacías (Día 2)
            VerificarCatalogo(catalogo);

            // 3. Estado inicial del catálogo antes de prestar (Día 3)
            Console.WriteLine("\n=== ESTADO DEL CATÁLOGO (ANTES DE LOS PRÉSTAMOS) ===");
            MostrarDisponibilidad(catalogo);

            // 4. Simulación de 3 préstamos (Día 3)
            Console.WriteLine("\n=== EJECUCIÓN DE 3 PRÉSTAMOS SIMULADOS ===");

            // Préstamo 1: "Patrones de Diseno"
            RealizarPrestamoSimulado(catalogo, "978-0201633610");

            // Préstamo 2: "Refactorizacion" (Libro con 1 solo ejemplar)
            RealizarPrestamoSimulado(catalogo, "978-0321125217");

            // Préstamo 3: Intento de volver a prestar "Refactorizacion" (Préstamo denegado)
            RealizarPrestamoSimulado(catalogo, "978-0321125217");

            // 5. Estado final del catálogo tras los préstamos (Día 3)
            Console.WriteLine("\n=== ESTADO DEL CATÁLOGO (DESPUÉS DE LOS PRÉSTAMOS) ===");
            MostrarDisponibilidad(catalogo);

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }

        // Método para cargar los 8 títulos de prueba
        static List<Titulo> CargarCatalogoInicial()
        {
            List<Titulo> catalogo = new List<Titulo>();
            catalogo.Add(new Titulo("978-0131103627", "El Lenguaje de Programacion C", 15, 5));
            catalogo.Add(new Titulo("978-0201633610", "Patrones de Diseno", 22, 3));
            catalogo.Add(new Titulo("978-0132350884", "Codigo Limpio", 22, 4)); // Empate en préstamos con Patrones
            catalogo.Add(new Titulo("978-0321125217", "Refactorizacion", 8, 1));
            catalogo.Add(new Titulo("978-0596007126", "Head First Design Patterns", 12, 2));
            catalogo.Add(new Titulo("978-0134494166", "Clean Architecture", 18, 5));
            catalogo.Add(new Titulo("978-0321278654", "Domain-Driven Design", 5, 1));
            catalogo.Add(new Titulo("978-0137081073", "The Clean Coder", 18, 2)); // Empate en préstamos con Clean Architecture
            return catalogo;
        }

        // Método para verificar que ningún título tenga celdas nulas (Día 2)
        static void VerificarCatalogo(List<Titulo> catalogo)
        {
            Console.WriteLine("=== VERIFICACIÓN DE CARGA Y AUSENCIA DE CELDAS VACÍAS ===\n");
            foreach (var t in catalogo)
            {
                Console.WriteLine($"ISBN: {t.ISBN} | Título: {t.Nombre}");
                Console.WriteLine($"  -> Préstamos Históricos: {t.PrestamosHistoricos}");
                Console.WriteLine($"  -> Cantidad Copias: {t.Ejemplares.Length} | Celdas nulas: {ContarNulos(t.Ejemplares)}");
                Console.WriteLine(new string('-', 60));
            }
        }

        static int ContarNulos(Ejemplar[] arreglo)
        {
            int nulos = 0;
            foreach (var item in arreglo)
            {
                if (item == null) nulos++;
            }
            return nulos;
        }

        // Método para realizar un préstamo y registrar su resultado
        static void RealizarPrestamoSimulado(List<Titulo> catalogo, string isbn)
        {
            Titulo libro = catalogo.Find(t => t.ISBN == isbn);
            if (libro != null)
            {
                bool exito = libro.RegistrarPrestamo();
                if (exito)
                {
                    Console.WriteLine($"[ÉXITO] Préstamo registrado: '{libro.Nombre}'. Quedan: {libro.ObtenerDisponibles()} copias | Préstamos totales: {libro.PrestamosHistoricos}");
                }
                else
                {
                    Console.WriteLine($"[DENEGADO] No hay ejemplares disponibles para: '{libro.Nombre}'.");
                }
            }
            else
            {
                Console.WriteLine($"[ERROR] Libro con ISBN {isbn} no fue encontrado.");
            }
        }

        // Método auxiliar para mostrar la tabla resumida de disponibilidad
        static void MostrarDisponibilidad(List<Titulo> catalogo)
        {
            foreach (var t in catalogo)
            {
                Console.WriteLine($"ISBN: {t.ISBN} | {t.Nombre.PadRight(32)} | Disponibles: {t.ObtenerDisponibles()}/{t.Ejemplares.Length} | Histórico: {t.PrestamosHistoricos}");
            }
        }
    }
}