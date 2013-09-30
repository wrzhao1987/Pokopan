using UnityEngine;
using System.Collections;


public class Cube : MonoBehaviour 
{
	public tk2dClippedSprite sprite;
	
	private Transform m_transform;
	private Vector3 destination;
	private Transform colliderTransform;
	private bool moving = false;

	public Transform ColliderTransform
	{
		get
		{
			return colliderTransform;
		}
	}
	
	void Start ()
	{
		m_transform = this.transform;
		colliderTransform = this.sprite.transform;
	}

	// Update is called once per frame
	void Update ()
	{
		
		if (moving)
		{
			Vector3 velocity = destination - m_transform.position;
			float damping = velocity.magnitude;
	
			if (velocity.sqrMagnitude < 0.3f)
			{
				moving = false;
				m_transform.position = destination;
			}
			else {
				velocity.Normalize();
				m_transform.position = (m_transform.position + (velocity * Time.deltaTime * (damping /0.5f)));
			}
		}
	}
	
	public void SetPosition(int column, int row)
	{
		sprite.transform.position  = new Vector3(column * 4 + 4.5f,  column % 2 == 0 ? 40 : 42, sprite.transform.position.z);
	}
	
	public void MoveTo(int column, int row)
	{
		moving = true;
		destination =  new Vector3(column * 4.25f + 3.2f, column % 2 == 0 ? row * 4.95f + 4.5f : row * 4.95f  + 6.8f, sprite.transform.position.z);
	}
	
	public void SetDestination(Vector3 destination)
	{
		this.destination = destination;
	}
	
}
