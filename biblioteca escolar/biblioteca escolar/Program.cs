using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BibliotecaEscolar
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Titulo> catalogo = CargarCatalogoInicial();

            Console.WriteLine("=================================================");
            Console.WriteLine("  ANÁLISIS Y COMPARACIÓN DE ALGORITMOS");
            Console.WriteLine("=================================================\n");

            // 1. Demostración de Ordenamiento Por Inserción (Manual)
            Console.WriteLine("--- 1. ORDENAMIENTO POR INSERCIÓN MANUAL (Por Préstamos) ---");
            OrdenarPorInsercionManual(catalogo);
            MostrarCatalogo(catalogo);

            // 2. Preparar catálogo ordenado por ISBN para las búsquedas binarias
            List<Titulo> catalogoPorISBN = new List<Titulo>(catalogo);
            OrdenarPorISBNManual(catalogoPorISBN);

            // 3. COMPARATIVA: Búsqueda Binaria Manual vs. Búsqueda Binaria Nativa (Array.BinarySearch)
            Console.WriteLine("\n--- 2. COMPARATIVA DE BÚSQUEDA BINARIA (ISBN) ---");
            string isbnPrueba = "978-0132350884"; // ISBN de "Codigo Limpio"

            // A) Búsqueda Binaria Manual (Tu algoritmo)
            int compManual;
            Stopwatch timerManual = Stopwatch.StartNew();
            int idxManual = BusquedaBinariaPorISBNManual(catalogoPorISBN, isbnPrueba, out compManual);
            timerManual.Stop();

            Console.WriteLine("[ALGORITMO MANUAL]");
            Console.WriteLine($"  -> Resultado: {(idxManual != -1 ? "Encontrado ('" + catalogoPorISBN[idxManual].Nombre + "')" : "No encontrado")}");
            Console.WriteLine($"  -> Comparaciones realizadas: {compManual}");
            Console.WriteLine($"  -> Tiempo estimado: {timerManual.Elapsed.TotalMilliseconds:F4} ms");

            // B) Búsqueda Binaria Nativa (C# Framework)
            // Creamos un arreglo de strings con los ISBNs para usar Array.BinarySearch
            string[] arreglosISBN = new string[catalogoPorISBN.Count];
            for (int i = 0; i < catalogoPorISBN.Count; i++)
            {
                arreglosISBN[i] = catalogoPorISBN[i].ISBN;
            }

            Stopwatch timerNativo = Stopwatch.StartNew();
            int idxNativo = Array.BinarySearch(arreglosISBN, isbnPrueba);
            timerNativo.Stop();

            Console.WriteLine("\n[MÉTODO NATIVO (Array.BinarySearch)]");
            Console.WriteLine($"  -> Resultado: {(idxNativo >= 0 ? "Encontrado en índice " + idxNativo : "No encontrado")}");
            Console.WriteLine($"  -> Tiempo estimado: {timerNativo.Elapsed.TotalMilliseconds:F4} ms");

            // 4. Conclusión Técnica en Consola
            Console.WriteLine("\n=================================================");
            Console.WriteLine("                ANÁLISIS DE EFICIENCIA");
            Console.WriteLine("=================================================");
            Console.WriteLine("• Búsqueda Binaria (Manual vs Nativa):");
            Console.WriteLine($"  Ambas técnicas logran una complejidad de O(log n). Para N=8 elementos,");
            Console.WriteLine($"  el número máximo teórico de comparaciones es log2(8) = 3 pasadas.");
            Console.WriteLine("• Ordenamiento por Inserción Manual:");
            Console.WriteLine($"  Complejidad O(n²) en el peor caso y O(n) en el mejor caso.");
            Console.WriteLine($"  Es un algoritmo ESTABLE ideal para catálogos pequeños.");
            Console.WriteLine("=================================================");

            Console.WriteLine("\nPresione cualquier tecla para finalizar el proyecto...");
            Console.ReadKey();
        }

        // --- CÁRGA INICIAL DE DATOS ---
        static List<Titulo> CargarCatalogoInicial()
        {
            List<Titulo> catalogo = new List<Titulo>();
            catalogo.Add(new Titulo("978-0131103627", "El Lenguaje de Programacion C", 15, 5));
            catalogo.Add(new Titulo("978-0201633610", "Patrones de Diseno", 22, 3));
            catalogo.Add(new Titulo("978-0132350884", "Codigo Limpio", 22, 4));
            catalogo.Add(new Titulo("978-0321125217", "Refactorizacion", 8, 1));
            catalogo.Add(new Titulo("978-0596007126", "Head First Design Patterns", 12, 2));
            catalogo.Add(new Titulo("978-0134494166", "Clean Architecture", 18, 5));
            catalogo.Add(new Titulo("978-0321278654", "Domain-Driven Design", 5, 1));
            catalogo.Add(new Titulo("978-0137081073", "The Clean Coder", 18, 2));
            return catalogo;
        }

        // --- ALGORITMOS DE ORDENAMIENTO MANUAL ---
        static void OrdenarPorInsercionManual(List<Titulo> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Titulo clave = lista[i];
                int j = i - 1;
                while (j >= 0 && !lista[j].DebeIrAntesQue(clave))
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = clave;
            }
        }

        static void OrdenarPorISBNManual(List<Titulo> lista)
        {
            for (int i = 1; i < lista.Count; i++)
            {
                Titulo clave = lista[i];
                int j = i - 1;
                while (j >= 0 && string.Compare(lista[j].ISBN, clave.ISBN, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = clave;
            }
        }

        // --- BÚSQUEDA BINARIA MANUAL CON CONTEO DE COMPARACIONES ---
        static int BusquedaBinariaPorISBNManual(List<Titulo> lista, string isbnBuscado, out int comparaciones)
        {
            int izquierda = 0;
            int derecha = lista.Count - 1;
            comparaciones = 0;

            while (izquierda <= derecha)
            {
                comparaciones++;
                int medio = izquierda + (derecha - izquierda) / 2;
                int comparacion = string.Compare(lista[medio].ISBN, isbnBuscado, StringComparison.OrdinalIgnoreCase);

                if (comparacion == 0)
                {
                    return medio; // Encontrado
                }
                else if (comparacion < 0)
                {
                    izquierda = medio + 1;
                }
                else
                {
                    derecha = medio - 1;
                }
            }

            return -1; // No encontrado
        }

        // --- MUESTRA DE CATÁLOGO ---
        static void MostrarCatalogo(List<Titulo> catalogo)
        {
            Console.WriteLine($"{"ISBN",-17} | {"Título",-30} | {"Préstamos",-10} | {"Disponibles"}");
            Console.WriteLine(new string('-', 72));
            foreach (var t in catalogo)
                Console.WriteLine($"{t.ISBN,-17} | {t.Nombre,-30} | {t.PrestamosHistoricos,-10} | {t.ObtenerDisponibles()}/{t.Ejemplares.Length}");
        }
    }
}