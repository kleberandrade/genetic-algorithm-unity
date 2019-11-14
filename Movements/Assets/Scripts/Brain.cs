using UnityEngine;

public class Brain : MonoBehaviour
{
    public int m_ChromosomeLength = 2;
    public Chromossome m_Chromosome;
    public Transform m_Eye;
    public float m_DistanceTravelled = 0.0f;
    private Vector3 m_StartPosition;
    public bool m_Alive;
    private bool m_SeeWall;
    public float m_Distance;
    public LayerMask m_LayerMask;

    public float m_Punishment = 0.5f;

    private PlayerController m_Controller;

    private void Start()
    {
        m_Controller = GetComponent<PlayerController>();
    }

    public void Initialize()
    {
        // Gene 0 - Forward
        // Gene 1 - Angle
        m_Chromosome = new Chromossome(2, 90);
        m_StartPosition = transform.position;
        m_Alive = true;
    }

    private void Update()
    {
        if (m_Chromosome == null)
            return;

        if (!m_Alive)
            return;

        RaycastHit hit;
        m_SeeWall = Physics.SphereCast(m_Eye.position, 0.75f, m_Eye.forward, out hit, m_Distance, m_LayerMask);
        Debug.DrawRay(m_Eye.position, m_Eye.forward * m_Distance, m_SeeWall ? Color.red : Color.white);
    }

    private void FixedUpdate()
    {
        if (m_Chromosome == null) 
            return;

        if (!m_Alive) 
            return;
        
        m_Controller.m_Vertical = m_Chromosome.GetGene(0);
        m_Controller.m_Horizontal = m_SeeWall ? m_Chromosome.GetGene(1) : 0.0f;
        m_DistanceTravelled = Vector3.Distance(m_StartPosition, transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            m_Alive = false;
            m_DistanceTravelled *= m_Punishment;
            m_Controller.m_Vertical = 0.0f;
            m_Controller.m_Horizontal = 0.0f;
        }
    }
}
