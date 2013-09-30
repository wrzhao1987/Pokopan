using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	static private int scoreCount;
	static private int berryCount;
	static private float timeCount;
	static private int enemyLife;
	static private float specialProgress = 0.5f;
	
	public Managing manageWhat;
	
	public enum Managing
	{
		TimeProgressBar = 1,
		EnemyLifeProgressBar,
		TimeCount,
		BerryCount,
		ScoreCount,
		SpecialProgressBar
	}
	
	static public int BerryCount
	{
		get
		{
			return berryCount;
		}
		set
		{
			berryCount = value;
		}
	}
	
	static public int ScoreCount
	{
		get
		{
			return scoreCount;
		}
		set
		{
			scoreCount = value;
		}
	}
	
	static public int TimeCount
	{
		get
		{
			return TimeCount;
		}
		set
		{
			timeCount = value;
		}
	}
	
	static public int EnemyLife
	{
		get
		{
			return enemyLife;
		}
		set
		{
			enemyLife = value;
		}
	}
	
	public float SpecialProgress
	{
		get
		{
			return specialProgress;
		}
		set
		{
			specialProgress = value;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		scoreCount = 0;
		berryCount = 0;
		timeCount = Manager.Instance.totalTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (manageWhat)
		{
		case Managing.TimeProgressBar:
			if (Manager.Instance.GameEnded)
			{
				Destroy(gameObject);
				return;
			}
			tk2dUIProgressBar progressBar = gameObject.GetComponent<tk2dUIProgressBar>();
			progressBar.Value -= Time.deltaTime / Manager.Instance.totalTime;
			break;
		
		case Managing.EnemyLifeProgressBar:
			tk2dUIProgressBar lifeBar = gameObject.GetComponent<tk2dUIProgressBar>();
			GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
			lifeBar.Value = (float)enemy.GetComponent<Enemy>().Life / (float)enemy.GetComponent<Enemy>().totalLife;
			break;
			
		case Managing.TimeCount:
			if (Manager.Instance.GameEnded)
			{
				return;
			}
			tk2dTextMesh timeDigit = gameObject.GetComponent<tk2dTextMesh>();
			timeCount = timeCount - Time.deltaTime;
			timeDigit.text = Mathf.CeilToInt(timeCount).ToString();
			timeDigit.Commit();
			if (timeCount <= 0.0f)
			{
				Manager.Instance.GameEnded = true;
				return;
			}
			break;
			
		case Managing.BerryCount:
			tk2dTextMesh berryDigit = gameObject.GetComponent<tk2dTextMesh>();
			berryDigit.text = berryCount.ToString();
			berryDigit.Commit();
			break;
			
		case Managing.ScoreCount:
			tk2dTextMesh scoreDigit = gameObject.GetComponent<tk2dTextMesh>();
			if (scoreCount >= Manager.Instance.Score)
			{
				scoreCount = Manager.Instance.Score;
				scoreDigit.text = Manager.Instance.Score.ToString();
			}
			else
			{
				scoreCount += 100;
				scoreDigit.text = scoreCount.ToString();
			}
			scoreDigit.Commit();
			break;
			
		case Managing.SpecialProgressBar:
			tk2dUIProgressBar specialBar = gameObject.GetComponent<tk2dUIProgressBar>();
			specialBar.Value = specialProgress;
			if (specialProgress <= 0.0f)
			{
				specialProgress = 0.0f;
			}
			specialProgress -= 0.0005f;
			break;
		}
		
		
	}
}