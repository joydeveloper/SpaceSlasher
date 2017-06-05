using UnityEngine;

/// <summary>
/// Class content methods for generate primitives with PerlinNoise allocation 
/// </summary>
public class PerlinNoisePlane : MonoBehaviour
{
    public float power = 3.0f;
    public float scale = 1.0f;
    private Vector2 startPoint = new Vector2(0f, 0f);

    private void Start()
    {
        MakeNoise();
    }

    private void MakeNoise()
    {
        MeshFilter mf = GetComponent<MeshFilter>(); // Ищем mesh
        Vector3[] vertices = mf.mesh.vertices; // Получаем его вершины
        for (int i = 0; i < vertices.Length; i++)
        {
            float x = startPoint.x + vertices[i].x * scale; // X координата вершины
            float z = startPoint.y + vertices[i].z * scale; // Z координата вершины
            vertices[i].y = (Mathf.PerlinNoise(x, z) - 0.5f) * power;  // Задаём высоту для точки с вышеуказанными координатами
        }
        mf.mesh.vertices = vertices; // Присваиваем вершины
        mf.mesh.RecalculateBounds(); // Обновляем вершины
        mf.mesh.RecalculateNormals(); // Обновляем нормали
    }
}