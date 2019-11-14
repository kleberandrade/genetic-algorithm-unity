using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject m_BlockPrefab;
    public int m_Width;
    public int m_Depth;
    public float m_Size;
    public float m_Noise = 0.2f;
    public float m_Height;

    private PopulationManager m_PopulationManager;
    private GameObject m_Begin;

    private void Awake()
    {
        m_PopulationManager = GetComponent<PopulationManager>();
    }

    private void Start()
    {
        float xTotal = (m_Width * m_Size - m_Size) * 0.5f;
        float zTotal = (m_Depth * m_Size - m_Size) * 0.5f;

        GameObject walls = new GameObject("Walls");
        walls.transform.parent = transform;

        for (int w = 0; w < m_Width; w++)
        {
            for (int d = 0; d < m_Depth; d++)
            {
                float x = -xTotal + w * m_Size;
                float z = -zTotal + d * m_Size;
                Vector3 position = new Vector3(x, m_Height, z);

                if (w == 1 && d == 1)
                {
                    m_Begin = new GameObject("Begin");
                    m_Begin.transform.position = position;
                    m_Begin.transform.parent = transform;
                    m_Begin.transform.SetSiblingIndex(1);
                }
                else 
                {
                    float number = Random.Range(0.0f, 1.0f);
                    if (w == 0 || d == 0 || w == m_Width - 1 || d == m_Depth - 1 || number < m_Noise)
                    {
                        GameObject wall = Instantiate(m_BlockPrefab, position, Quaternion.identity);
                        wall.transform.parent = walls.transform;
                    }
                }
            }
        }

        m_PopulationManager.Initialize(m_Begin.transform);
    }
}
