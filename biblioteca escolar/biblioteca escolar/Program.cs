using System;
using System.Collections.Generic;

namespace BibliotecaEscolar
{
    class Program
    {
        static void Main(string[] args)
        {
            // Inicialización del catálogo como List<Titulo>
            List<Titulo> catalogo = new List<Titulo>();

            // Carga de 8 títulos con copias irregulares (1 a 5 ejemplares)
            catalogo.Add(new Titulo("978-0131103627", "El Lenguaje de Programacion C", 15, 5));
            catalogo.Add(new Titulo("978-0201633610", "Patrones de Diseno", 22, 3));
            catalogo.Add(new Titulo("978-0132350884", "Codigo Limpio", 22, 4)); // Empate en préstamos con Patrones
            catalogo.Add(new Titulo("978-0321125217", "Refactorizacion", 8, 1));
            catalogo.Add(new Titulo("978-0596007126", "Head First Design Patterns", 12, 2));
            catalogo.Add(new Titulo("978-0134494166", "Clean Architecture", 18, 5));
            catalogo.Add(new Titulo("978-0321278654", "Domain-Driven Design", 5, 1));
            catalogo.Add(new Titulo("978-0137081073", "The Clean Coder", 18, 2)); // Empate en préstamos con Clean Architecture

            // Verificación en consola
            VerificarCatalogo(catalogo);
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
    }
}