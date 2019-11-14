using System.Collections.Generic;
using UnityEngine;

public class Chromossome 
{
    private List<int> m_Genes = new List<int>();
    private int m_Length;
    private int m_MaxValue;

    public Chromossome(int length, int maxValue)
    {
        m_Length = length;
        m_MaxValue = maxValue;
        SetRandom();
    }

    public void SetRandom()
    {
        m_Genes.Clear();
        for (int i = 0; i < m_Length; i++)
        {
            int gene = Random.Range(0, m_MaxValue);
            m_Genes.Add(gene);
        }
    }

    public int GetGene(int position)
    {
        return m_Genes[position];
    }
    
    public void SetGene(int position, int value)
    {
        m_Genes[position] = value;
    }

    public void Mutate()
    {
        int position = Random.Range(0, m_Length);
        int value = Random.Range(0, m_MaxValue);
        m_Genes[position] =  value;
    }

    public void Crossover(Chromossome c1, Chromossome c2)
    {
        for (int i = 0; i < m_Length; i++)
        {
            m_Genes[i] = Random.Range(0.0f, 1.0f) < 0.5f ? c1.m_Genes[i] : c2.m_Genes[i];
        }
    }

    public override string ToString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        builder.Append("[");
        for (int i = 0; i < m_Length; i++)
        {
            builder.Append(m_Genes[i].ToString("0000")).Append("\t");
        }
        builder.Append("]");

        return builder.ToString();
    }
}
