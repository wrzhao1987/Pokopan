using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public int totalLife;
	private int life;
	
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
	void Start () {
		life = totalLife;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void  OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("Bullet") != 0)
		{
			return;
		}
		int power = other.transform.GetComponent<Bullet>().power;
		Life  = Life - power;
		Manager.Instance.Score = Manager.Instance.Score + 100;
		if (Life <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
