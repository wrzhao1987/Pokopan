using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	
	private Transform m_transform;
	private Vector3 destination = Vector3.zero;
	public ParticleSystem m_particle;
	public int power;
	private float accer = 10;
	private GameObject particleObject;
	
	// Use this for initialization
	void Start () 
	{
		m_transform = this.transform;
		particleObject = m_transform.Find("BulletParticle").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameObject.FindGameObjectWithTag("Enemy") == null)
		{
			Destroy(this.gameObject);
		}
		if (destination != Vector3.zero)
		{
			Vector3 velocity = destination - m_transform.position;
			velocity.Normalize();
			m_transform.position = (m_transform.position + velocity * Time.deltaTime * accer);
			 accer += 0.3f;
			m_transform.Rotate(Vector3.forward, Time.deltaTime * 500);
		}
		particleObject.transform.LookAt(-Vector3.up);
	}
	
	public void SetDestination(Vector3 enemyPosition)
	{
		destination = enemyPosition;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("Enemy") != 0)
		{
			return;
		}
		Destroy(this.gameObject);
	}
}
