using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached finish line!");

            
            winScreen.SetActive(true);

        
            Time.timeScale = 0f;
        }
    }
}
