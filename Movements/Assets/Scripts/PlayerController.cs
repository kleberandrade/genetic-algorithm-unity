using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Brain Inputs")]
    public float m_Vertical;
    public float m_Horizontal;


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(0.0f, 0.0f, m_Vertical) * Time.deltaTime;
        Vector3 rotation = new Vector3(0.0f, m_Horizontal, 0.0f) * Time.deltaTime;

        transform.Rotate(rotation);
        transform.Translate(movement);
    }
}
