using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public int totalLife;
	private int life;
	
	private Animation m_ani;
	
	public int Life
	{
		get
		{
			return life;
		}
		set
		{
			life = value;
		}
	}
	// Use this for initialization
	void Start ()
	{
		m_ani = this.gameObject.GetComponent<Animation>();
		life = totalLife;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (! m_ani.IsPlaying("EnemyNormal") && ! m_ani.IsPlaying("EnemyHit"))
		{
			m_ani.Play("EnemyNormal");
		}
	}
	
	void  OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("Bullet") == 0)
		{
			m_ani.Play("EnemyHit");
			int power = other.transform.GetComponent<Bullet>().power;
			Life  = Life - power;
			Manager.Instance.Score = Manager.Instance.Score + 100;
		}
		else if (other.tag.CompareTo("Rabbit") == 0)
		{
			m_ani.Play("EnemyHit");	
			Manager.Instance.Score = Manager.Instance.Score + 10000;
			Life = 0;
		}
		if (Life <= 0)
		{
			Manager.Instance.CurrentEnemyLevel = Manager.Instance.CurrentEnemyLevel + 1;
			Destroy(this.gameObject);
		}
	}
}
