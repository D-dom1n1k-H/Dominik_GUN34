using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField]
	private Vector3 _start;
	[SerializeField]
    private Vector3 _end;
	[SerializeField]
	private float _speed;
	[SerializeField]
	private float _delay;

    private IEnumerator Start()
    {
		if (_speed <= 0.1f) { _speed = 6f; }
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb == null) { Debug.LogError("Rigidbody missing in Mover"); }

        while (true)
			{
			if (Vector3.Distance(transform.position, _start) > 0.01f)
			{
				while (transform.position != _start)
				{
					Vector3 nextPos = Vector3.MoveTowards(rb.position, _start, _speed * Time.fixedDeltaTime);
					rb.MovePosition(nextPos);
                    yield return new WaitForFixedUpdate();
                }
            }
			yield return new WaitForSeconds(_delay);

				if (Vector3.Distance(transform.position, _end) > 0.01f)
				{
					while (transform.position != _end)
					{
                    Vector3 nextPos = Vector3.MoveTowards(rb.position, _end, _speed * Time.fixedDeltaTime);
                    rb.MovePosition(nextPos);
                    yield return new WaitForFixedUpdate();
					}
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
		Gizmos.DrawLine(_start, _end);

		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(_start, 0.5f);
		Gizmos.DrawSphere(_end, 0.5f);
    }
}
