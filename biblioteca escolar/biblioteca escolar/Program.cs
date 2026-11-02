using System;
using System.Collections.Generic;

namespace BibliotecaEscolar
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Cargar catálogo inicial 
            List<Titulo> catalogo = CargarCatalogoInicial();

            // 2. Ordenamiento por inserción manual sobre datos de prueba puros (Día 4)
            Console.WriteLine("=== CATÁLOGO ORDENADO POR PRÉSTAMOS (DE MAYOR A MENOR) ===");
            OrdenarPorInsercionManual(catalogo);
            MostrarCatalogoOrdenado(catalogo);

            // 3. Verificación de Estabilidad 
            VerificarEstabilidadEmpates(catalogo);

            // 4. Simulación de los 3 préstamos
            Console.WriteLine("\n=== EJECUCIÓN DE 3 PRÉSTAMOS SIMULADOS ===");
            RealizarPrestamoSimulado(catalogo, "978-0201633610");
            RealizarPrestamoSimulado(catalogo, "978-0321125217");
            RealizarPrestamoSimulado(catalogo, "978-0321125217");

            // 5. Re-ordenar para mostrar cómo queda el catálogo tras alterar contadores
            Console.WriteLine("\n=== CATÁLOGO RE-ORDENADO DESPUÉS DE LOS PRÉSTAMOS ===");
            OrdenarPorInsercionManual(catalogo);
            MostrarCatalogoOrdenado(catalogo);

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }

       

        static List<Titulo> CargarCatalogoInicial()
        {
            List<Titulo> catalogo = new List<Titulo>();
            catalogo.Add(new Titulo("978-0131103627", "El Lenguaje de Programacion C", 15, 5));
            catalogo.Add(new Titulo("978-0201633610", "Patrones de Diseno", 22, 3));        // Empate en 22 préstamos
            catalogo.Add(new Titulo("978-0132350884", "Codigo Limpio", 22, 4));             // Empate en 22 préstamos
            catalogo.Add(new Titulo("978-0321125217", "Refactorizacion", 8, 1));
            catalogo.Add(new Titulo("978-0596007126", "Head First Design Patterns", 12, 2));
            catalogo.Add(new Titulo("978-0134494166", "Clean Architecture", 18, 5));        // Empate en 18 préstamos
            catalogo.Add(new Titulo("978-0321278654", "Domain-Driven Design", 5, 1));
            catalogo.Add(new Titulo("978-0137081073", "The Clean Coder", 18, 2));          // Empate en 18 préstamos
            return catalogo;
        }

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

        static void MostrarDisponibilidad(List<Titulo> catalogo)
        {
            foreach (var t in catalogo)
            {
                Console.WriteLine($"ISBN: {t.ISBN} | {t.Nombre.PadRight(32)} | Disponibles: {t.ObtenerDisponibles()}/{t.Ejemplares.Length} | Histórico: {t.PrestamosHistoricos}");
            }
        }

        

        // Ordenamiento por Inserción propio (sin Array.Sort ni List.Sort)
        static void OrdenarPorInsercionManual(List<Titulo> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Titulo clave = lista[i];
                int j = i - 1;

                // Desplaza elementos hacia la derecha si no cumplen con ir antes que la clave
                while (j >= 0 && !lista[j].DebeIrAntesQue(clave))
                {
                    lista[j + 1] = lista[j];
                    j--;
                }

                lista[j + 1] = clave;
            }
        }

        static void MostrarCatalogoOrdenado(List<Titulo> catalogo)
        {
            Console.WriteLine($"{"ISBN",-17} | {"Título",-32} | {"Préstamos",-10} | {"Disponibles",-12}");
            Console.WriteLine(new string('-', 80));
            foreach (var t in catalogo)
            {
                Console.WriteLine($"{t.ISBN,-17} | {t.Nombre,-32} | {t.PrestamosHistoricos,-10} | {t.ObtenerDisponibles()}/{t.Ejemplares.Length}");
            }
        }

        static void VerificarEstabilidadEmpates(List<Titulo> catalogo)
        {
            Console.WriteLine("\n=== VERIFICACIÓN DE ESTABILIDAD (ORDEN ALFABÉTICO EN EMPATES) ===");
            for (int i = 0; i < catalogo.Count - 1; i++)
            {
                if (catalogo[i].PrestamosHistoricos == catalogo[i + 1].PrestamosHistoricos)
                {
                    Console.WriteLine($"[EMPATE DETECTADO en {catalogo[i].PrestamosHistoricos} préstamos]:");
                    Console.WriteLine($"  Posición {i + 1}: {catalogo[i].Nombre}");
                    Console.WriteLine($"  Posición {i + 2}: {catalogo[i + 1].Nombre}");
                    bool correcto = string.Compare(catalogo[i].Nombre, catalogo[i + 1].Nombre, StringComparison.OrdinalIgnoreCase) <= 0;
                    Console.WriteLine($"  -> ¿Mantiene orden alfabético?: {(correcto ? "SÍ (Estable)" : "NO")}\n");
                }
            }
        }
    }
}