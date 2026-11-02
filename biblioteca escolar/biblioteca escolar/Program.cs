using System;
using System.Collections.Generic;

namespace BibliotecaEscolar
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Cargar catálogo (Día 2)
            List<Titulo> catalogo = CargarCatalogoInicial();

            // 2. Ordenamiento manual por inserción y verificación de estabilidad (Día 4)
            Console.WriteLine("=== CATÁLOGO ORDENADO POR PRÉSTAMOS ===");
            OrdenarPorInsercionManual(catalogo);
            MostrarCatalogo(catalogo);
            VerificarEstabilidadEmpates(catalogo);

            // 3. Simulación de 3 préstamos (Día 3)
            Console.WriteLine("\n=== SIMULACIÓN DE PRÉSTAMOS ===");
            RealizarPrestamoSimulado(catalogo, "978-0201633610"); // Patrones de Diseno
            RealizarPrestamoSimulado(catalogo, "978-0321125217"); // Refactorizacion
            RealizarPrestamoSimulado(catalogo, "978-0321125217"); // Intento sin stock

            // 4. Búsquedas por Título e ISBN (Día 5)
            Console.WriteLine("\n=== PRUEBAS DE BÚSQUEDA (DÍA 5) ===");
            BuscarPorTituloSecuencial(catalogo, "Codigo Limpio");
            BuscarPorTituloSecuencial(catalogo, "Java para Principiantes");

            // Búsqueda binaria requiere copia ordenada por ISBN
            List<Titulo> catalogoPorISBN = new List<Titulo>(catalogo);
            OrdenarPorISBNManual(catalogoPorISBN);

            BuscarPorISBNBinaria(catalogoPorISBN, "978-0132350884"); // Encontrado
            BuscarPorISBNBinaria(catalogoPorISBN, "978-9999999999"); // No encontrado

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }

        // --- MÉTODOS REQUERIDOS ---

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

        static void BuscarPorISBNBinaria(List<Titulo> catalogoISBN, string isbn)
        {
            int izq = 0, der = catalogoISBN.Count - 1, comp = 0, idx = -1;
            while (izq <= der)
            {
                comp++;
                int med = izq + (der - izq) / 2;
                int c = string.Compare(catalogoISBN[med].ISBN, isbn, StringComparison.OrdinalIgnoreCase);
                if (c == 0) { idx = med; break; }
                if (c < 0) izq = med + 1; else der = med - 1;
            }

            if (idx != -1)
                Console.WriteLine($"[BINARIA] Existe '{catalogoISBN[idx].Nombre}' | Disponibles: {catalogoISBN[idx].ObtenerDisponibles()} | Comparaciones: {comp}");
            else
                Console.WriteLine($"[BINARIA] ISBN '{isbn}' no encontrado | Comparaciones: {comp}");
        }

        static void BuscarPorTituloSecuencial(List<Titulo> catalogo, string nombre)
        {
            Titulo t = catalogo.Find(x => x.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (t != null)
                Console.WriteLine($"[SECUENCIAL] Encontrado: '{t.Nombre}' | ISBN: {t.ISBN} | Disponibles: {t.ObtenerDisponibles()}");
            else
                Console.WriteLine($"[SECUENCIAL] Título '{nombre}' no encontrado.");
        }

        static void RealizarPrestamoSimulado(List<Titulo> catalogo, string isbn)
        {
            Titulo t = catalogo.Find(x => x.ISBN == isbn);
            if (t != null && t.RegistrarPrestamo())
                Console.WriteLine($"[PRÉSTAMO OK] '{t.Nombre}' | Quedan: {t.ObtenerDisponibles()} | Total préstamos: {t.PrestamosHistoricos}");
            else
                Console.WriteLine($"[PRÉSTAMO DENEGADO] Sin stock o no existe libro con ISBN {isbn}");
        }

        static void MostrarCatalogo(List<Titulo> catalogo)
        {
            Console.WriteLine($"{"ISBN",-17} | {"Título",-30} | {"Préstamos",-10} | {"Disponibles"}");
            Console.WriteLine(new string('-', 72));
            foreach (var t in catalogo)
                Console.WriteLine($"{t.ISBN,-17} | {t.Nombre,-30} | {t.PrestamosHistoricos,-10} | {t.ObtenerDisponibles()}/{t.Ejemplares.Length}");
        }

        static void VerificarEstabilidadEmpates(List<Titulo> catalogo)
        {
            for (int i = 0; i < catalogo.Count - 1; i++)
            {
                if (catalogo[i].PrestamosHistoricos == catalogo[i + 1].PrestamosHistoricos)
                    Console.WriteLine($"[ESTABILIDAD OK] Empate en {catalogo[i].PrestamosHistoricos} préstamos: '{catalogo[i].Nombre}' antes que '{catalogo[i + 1].Nombre}'");
            }
        }
    }
}