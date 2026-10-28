# Tarea 1.2 — Memoria estática y memoria dinámica en C#

**Materia:** Estructuras de Datos | **Unidad:** 1 | **Autor:** Jesús Armando Zendejo Altamirano

## Requisitos
- .NET SDK 8.0 o superior compatible con el proyecto.
- Windows con PowerShell, Visual Studio, Visual Studio Code o terminal con .NET.

## Ejecución
1. Abre la carpeta del proyecto en Visual Studio Code o en una terminal.
2. Ejecuta:

```powershell
dotnet run --configuration Release
```

3. Verifica las tres secciones:
   - `CASO 1`: un arreglo de longitud 3 crece a 5 y disminuye a 2. El original no cambia.
   - `CASO 2`: tres nodos de una lista enlazada se conectan por referencias.
   - `CASO 3`: mediciones orientativas de tiempo y bytes asignados para 6,000 enteros.
4. El resultado debe terminar con `Comprobación de integridad: CORRECTA`.

## Capturas para el reporte
Toma las capturas **en tu propio equipo**, con Win + Shift + S, guardándolas en `capturas/`:

- `01_arreglo.png`: salida desde `CASO 1` hasta antes de `CASO 2`.
- `02_lista.png`: salida del `CASO 2`.
- `03_medicion.png`: tabla del `CASO 3` con los valores **reales** obtenidos en tu equipo.

Pega estas tres imágenes en los espacios indicados del reporte en Word. Las cifras de tiempo y asignaciones variarán con el sistema y entre ejecuciones.

## Interpretación correcta
- "Estático" en la tarea se usa para **capacidad fija de la estructura**, no para afirmar que `int[]` esté almacenado en el *Stack* ni para confundirlo con `static` de C#.
- `Array.Resize` crea un arreglo distinto si la longitud cambia; no extiende la instancia original.
- Cada `Nodo` se crea con `new` en el *heap* administrado; las referencias conectan los nodos y .NET administra su liberación mediante GC.
- `GC.GetAllocatedBytesForCurrentThread` informa **bytes acumulados asignados durante el intervalo**, no el consumo de memoria residente ni los bytes aún en uso.
- El ejemplo de redimensionar **en cada inserción** muestra una estrategia deliberadamente costosa; otras colecciones dinámicas como `List<T>` reservan capacidad adicional y no necesariamente copian en cada inserción.

## Subirlo a GitHub
1. Crea un repositorio nuevo, por ejemplo `estructuras-datos-tarea-1-2`.
2. Sube `Program.cs`, `Tarea_1_2_Memoria_CSharp.csproj`, `README.md` y `.gitignore` (capturas opcionales).
3. Copia la URL pública del repositorio en la entrega escolar.

## Fuentes
- Microsoft Learn: https://learn.microsoft.com/es-es/dotnet/api/system.array.resize
- Microsoft Learn: https://learn.microsoft.com/es-es/dotnet/standard/garbage-collection/fundamentals
- Microsoft Learn: https://learn.microsoft.com/es-es/dotnet/api/system.gc.getallocatedbytesforcurrentthread
