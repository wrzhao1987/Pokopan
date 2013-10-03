using UnityEngine;
using System.Collections;

public class Rabbit : MonoBehaviour {
	
	private Animation m_ani;
	private Vector3 destination = Vector3.zero;
	private Transform m_transform;
	private float accer = 10;
	
	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		m_ani = gameObject.GetComponent<Animation>();
		m_ani.Play("RabbitNormal");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (destination != Vector3.zero)
		{
			m_ani.Stop("RabbitNormal");
			m_transform.Rotate(-Vector3.forward, Time.deltaTime * 50);
			Vector3 velocity = destination - m_transform.position;
			velocity.Normalize();
			m_transform.position = (m_transform.position + velocity * Time.deltaTime * accer);
			accer += 0.1f;
		}
	}
	
	public void RushToEnemy()
	{
		GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
		
		if (enemyObject != null)
		{
			destination = enemyObject.transform.position;
		}
	}
	
	void  OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("Enemy") == 0)
		{
			Destroy(this.gameObject);
		}
		return;
	}
}
