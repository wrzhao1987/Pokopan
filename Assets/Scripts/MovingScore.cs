using UnityEngine;
using System.Collections;

public class MovingScore : MonoBehaviour 
{
	public int lifeTime;
	private Transform m_transform;
	private float livedTime = 0.0f;
	private int score;
	
	public int Score
	{
		get
		{
			return this.score;
		}
		set
		{
			score = value;
		}
	}
	// Use this for initialization
	void Start () 
	{
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		tk2dTextMesh movingScore = gameObject.GetComponent<tk2dTextMesh>();
		movingScore.text = score.ToString();
		movingScore.Commit();
		livedTime += Time.deltaTime;
		if (livedTime <= lifeTime)
		{
			m_transform.Translate(Vector3.up * Time.deltaTime * 2);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
}
