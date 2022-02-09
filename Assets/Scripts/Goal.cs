using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private GameObject levelFinishedPanel;

    void Start()
    {
        levelFinishedPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelFinishedPanel.SetActive(true);
        }
    }
}
