using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    [Header("Chromossome")]
    public GameObject m_Prefab;

    [Header("Parameters")]
    public int m_PopulationSize = 50;
    public int m_TournamentSelectionSize = 3;
    public float m_MutationRate = 0.01f;
    public int m_TrialTime = 10;

    [Header("UI (User Interface)")]
    public Text m_GenerationText;
    public Text m_DeadsText;
    public Text m_ElapsedTimeText;
    public Text m_AverageText;
    public Text m_MaxText;

    private List<Brain> m_Population = new List<Brain>();
    private float m_ElapsedTime = 0;
    private int m_Generation = 1;

    private bool m_Initialized;

    private Transform m_Begin;

    private float m_AvgFitness = 0.0f;
    private float m_MaxFitness = 0.0f;
    private int m_Deads = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void Initialize(Transform begin)
    {
        m_Begin = begin;

        for (int i = 0; i < m_PopulationSize; i++)
        {
            GameObject bot = Instantiate(m_Prefab, m_Begin.position, m_Begin.rotation);
            Brain brain = bot.GetComponent<Brain>();
            brain.Initialize();
            m_Population.Add(brain);
        }

        m_Initialized = true;
    }

    private void Update()
    {
        if (!m_Initialized)
            return;

        m_ElapsedTime += Time.deltaTime;
        m_Deads = m_Population.Where(x => x.m_Alive).ToList().Count;

        if (m_ElapsedTime > m_TrialTime || m_Deads == 0)
        {
            NewPopulation();
            m_ElapsedTime = 0;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        m_GenerationText.text = $"Generation: {m_Generation}";
        m_ElapsedTimeText.text = $"Trial Time: {(int)m_ElapsedTime}";
        m_AverageText.text = $"Avg Fitness: {m_AvgFitness.ToString("0.00")}";
        m_MaxText.text = $"Max Fitness: {m_MaxFitness.ToString("0.00")}";
        m_DeadsText.text = $"Deads: {m_PopulationSize - m_Deads}/{m_PopulationSize}";
    }

    public Brain TournamentSelection()
    {
        List<Brain> candidates = new List<Brain>();

        for (int i = 0; i < m_TournamentSelectionSize; i++)
        {
            int index = Random.Range(0, m_PopulationSize - 1);
            candidates.Add(m_Population[index]);
        }

        return candidates.OrderByDescending(x => x.m_DistanceTravelled).ToList()[0];
    }

    private void Evaluate()
    {
        m_AvgFitness = 0.0f;
        m_MaxFitness = 0.0f;
        for (int i = 0; i < m_PopulationSize; i++)
        {
            float distance = m_Population[i].GetComponent<Brain>().m_DistanceTravelled;
            m_AvgFitness += distance;
            m_MaxFitness = Mathf.Max(distance, m_MaxFitness);
        }
        
        m_AvgFitness = m_AvgFitness / (float)m_PopulationSize;
    }

    private void NewPopulation()
    {
        Evaluate();

        List<Brain> newPopulation = new List<Brain>();
        for (int i = 0; i < m_PopulationSize; i++)
        {
            Chromossome parent1 = TournamentSelection().m_Chromosome;
            Chromossome parent2 = TournamentSelection().m_Chromosome;

            GameObject offspring = Instantiate(m_Prefab, m_Begin.position, m_Begin.rotation);
            Brain brain = offspring.GetComponent<Brain>();
            brain.Initialize();

            brain.m_Chromosome.Crossover(parent1, parent2);

            if (Random.Range(0.0f, 1.0f) < m_MutationRate)
                brain.m_Chromosome.Mutate();

            newPopulation.Add(brain);
        }

        for (int i = 0; i < m_PopulationSize; i++)
            Destroy(m_Population[i].gameObject);

        m_Population = newPopulation;
        m_Generation++;
    }
}
