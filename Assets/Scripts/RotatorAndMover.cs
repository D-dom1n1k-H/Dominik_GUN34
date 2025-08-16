
using System.Collections;
using UnityEngine;

public class RotatorAndMover : MonoBehaviour
{
    private Vector3 _start = new Vector3(-10f, 1.12f, -20f);
    private Vector3 _end = new Vector3(10f, -1.12f, 20f);
    [SerializeField, ReadOnly]
    private Vector3 _nextPosition;
    [SerializeField]
    private Vector3 _rotate;

    private IEnumerator Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) { Debug.LogError("Rigitbody missing in RotatorAndMover"); }

        while (true)
        {
            _nextPosition = GetNextPosition();

            while (Vector3.Distance(transform.position, _nextPosition) > 0.05f)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotate * Time.fixedDeltaTime));

                Vector3 target = Vector3.MoveTowards(rb.position, _nextPosition, 6f * Time.fixedDeltaTime);
                rb.MovePosition(new Vector3(target.x, rb.position.y, target.z)); 

                yield return new WaitForFixedUpdate();
            }
        }
    }

    private Vector3 GetNextPosition()
    {
        float randomX = UnityEngine.Random.Range(_start.x, _end.x);
        float randomZ = UnityEngine.Random.Range(_start.z, _end.z);
        Vector3 randomPosition = new Vector3(randomX, _start.y, randomZ);

        return _nextPosition = randomPosition;
    }
}
