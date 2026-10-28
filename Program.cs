using System;
using System.Diagnostics;
using System.Text;

// Tarea 1.2 - Estructuras de Datos - Unidad 1.
// Demuestra arreglos de tamaño fijo, Array.Resize y objetos en el heap administrado.

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("TAREA 1.2 | MEMORIA ESTÁTICA Y DINÁMICA EN C#");
Console.WriteLine(new string('=', 52));

// CASO 1: arreglo de longitud fija y redimensionamiento.
int[] arregloOriginal = { 10, 20, 30 };
int[] arregloModificado = arregloOriginal;

Console.WriteLine("\nCASO 1. ARREGLO Y REDIMENSIONAMIENTO");
MostrarArreglo("Arreglo original", arregloOriginal);

// Array.Resize crea un arreglo nuevo cuando cambia la longitud;
// el objeto original conserva sus tres posiciones.
Array.Resize(ref arregloModificado, 5);
arregloModificado[3] = 40;
arregloModificado[4] = 50;
MostrarArreglo("Después de crecer a 5", arregloModificado);
Console.WriteLine($"¿Es la misma instancia?: {ReferenceEquals(arregloOriginal, arregloModificado)}");
MostrarArreglo("Original sin cambios", arregloOriginal);

// Al reducir, se conservan solo los valores que caben.
Array.Resize(ref arregloModificado, 2);
MostrarArreglo("Después de reducir a 2", arregloModificado);

// CASO 2: memoria dinámica por medio de objetos y referencias.
Console.WriteLine("\nCASO 2. OBJETOS EN HEAP (LISTA ENLAZADA)");
ListaEnlazada lista = new();
lista.Agregar(10);
lista.Agregar(20);
lista.Agregar(30);
Console.WriteLine($"Nodos creados: {lista.Cantidad}");
Console.WriteLine($"Valores de la lista: {lista.ValoresComoTexto()}");
Console.WriteLine("Los nodos son instancias conectadas mediante referencias.");
Console.WriteLine("El recolector de basura (.NET GC) administra su memoria.");

// CASO 3: comparativa sencilla. Los resultados dependen del equipo.
// Se usa un tamaño moderado para que la práctica termine con rapidez.
const int n = 6_000;
Console.WriteLine($"\nCASO 3. MEDICIÓN (n = {n} ENTEROS)");

// Calentamiento: reduce el efecto de la compilación JIT en los tiempos.
_ = LlenarArregloFijo(100);
_ = LlenarRedimensionando(100);
_ = LlenarLista(100);

Resultado fijo = Medir(() => LlenarArregloFijo(n));
Resultado redimensionado = Medir(() => LlenarRedimensionando(n));
Resultado enlazado = Medir(() => LlenarLista(n));

Console.WriteLine($"{"Método",-25} {"Tiempo (ms)",13} {"Asignado (KiB)",16} {"Suma",12}");
MostrarResultado("Arreglo preasignado", fijo);
MostrarResultado("Resize en cada inserción", redimensionado);
MostrarResultado("Lista de nodos", enlazado);

bool datosCorrectos = fijo.Suma == redimensionado.Suma
                      && fijo.Suma == enlazado.Suma
                      && fijo.Suma == (long)n * (n - 1) / 2;
Console.WriteLine($"\nComprobación de integridad: {(datosCorrectos ? "CORRECTA" : "ERROR")}");
Console.WriteLine("Nota: son mediciones orientativas de una ejecución, no un benchmark científico.");

static void MostrarArreglo(string nombre, int[] datos)
{
    Console.WriteLine($"{nombre}: [{string.Join(", ", datos)}] | longitud = {datos.Length}");
}

static void MostrarResultado(string nombre, Resultado resultado)
{
    double kib = resultado.BytesAsignados / 1024.0;
    Console.WriteLine($"{nombre,-25} {resultado.Milisegundos,13:F3} {kib,16:F1} {resultado.Suma,12}");
}

// Mide únicamente el trabajo definido por la función recibida.
// BytesAsignados mide asignaciones del hilo, no memoria que sigue en uso.
static Resultado Medir(Func<long> trabajo)
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    long bytesAntes = GC.GetAllocatedBytesForCurrentThread();
    long inicio = Stopwatch.GetTimestamp();
    long suma = trabajo();
    long fin = Stopwatch.GetTimestamp();
    long bytesDespues = GC.GetAllocatedBytesForCurrentThread();

    double ms = (fin - inicio) * 1000.0 / Stopwatch.Frequency;
    return new Resultado(ms, bytesDespues - bytesAntes, suma);
}

static long LlenarArregloFijo(int cantidad)
{
    int[] datos = new int[cantidad];
    for (int i = 0; i < cantidad; i++)
        datos[i] = i;

    long suma = 0;
    foreach (int valor in datos)
        suma += valor;
    return suma;
}

static long LlenarRedimensionando(int cantidad)
{
    int[] datos = Array.Empty<int>();
    for (int i = 0; i < cantidad; i++)
    {
        // Cada inserción que cambia la longitud crea y copia un arreglo.
        Array.Resize(ref datos, datos.Length + 1);
        datos[i] = i;
    }

    long suma = 0;
    foreach (int valor in datos)
        suma += valor;
    return suma;
}

static long LlenarLista(int cantidad)
{
    ListaEnlazada lista = new();
    for (int i = 0; i < cantidad; i++)
        lista.Agregar(i);
    return lista.SumarValores();
}

internal readonly record struct Resultado(double Milisegundos, long BytesAsignados, long Suma);

// Nodo: cada instancia es un objeto del heap administrado.
internal sealed class Nodo
{
    public int Valor { get; }
    public Nodo? Siguiente { get; set; }

    public Nodo(int valor) => Valor = valor;
}

// Los elementos se conectan con referencias, sin punteros inseguros.
internal sealed class ListaEnlazada
{
    private Nodo? cabeza;
    private Nodo? cola;
    public int Cantidad { get; private set; }

    public void Agregar(int valor)
    {
        Nodo nuevo = new(valor);
        if (cabeza is null)
            cabeza = nuevo;
        else
            cola!.Siguiente = nuevo;

        cola = nuevo;
        Cantidad++;
    }

    public string ValoresComoTexto()
    {
        StringBuilder texto = new();
        for (Nodo? actual = cabeza; actual is not null; actual = actual.Siguiente)
        {
            if (texto.Length > 0) texto.Append(" -> ");
            texto.Append(actual.Valor);
        }
        return texto.ToString();
    }

    public long SumarValores()
    {
        long suma = 0;
        for (Nodo? actual = cabeza; actual is not null; actual = actual.Siguiente)
            suma += actual.Valor;
        return suma;
    }
}
