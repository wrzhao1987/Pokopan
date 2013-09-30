using UnityEngine;
using System.Collections;

public class Berry: MonoBehaviour {
	
	private Transform m_transform;
	private Vector3 destination = Vector3.zero;
	private float accer = 10;
	
	// Use this for initialization
	void Start () 
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (destination != Vector3.zero)
		{
			Vector3 velocity = destination - m_transform.position;
			velocity.Normalize();
			m_transform.position = (m_transform.position + velocity * Time.deltaTime * accer);
			accer += 0.3f;
			m_transform.Rotate(Vector3.forward, Time.deltaTime * 500);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("BerryReceiver") != 0)
		{
			return;
		}
		Destroy(this.gameObject);
		Score.BerryCount = Score.BerryCount + 1;
	}

	public void SetDestination(Vector3 receiverPosition)
	{
		destination = receiverPosition;
	}
}
