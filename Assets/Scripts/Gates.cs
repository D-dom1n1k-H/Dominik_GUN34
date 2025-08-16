
using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private int _score = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ball")
        {
            _score++;
            Debug.Log("Score: " + _score);
            Destroy(other.gameObject);
        }
    }
}
