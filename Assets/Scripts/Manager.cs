using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager: MonoBehaviour {
	
	static private bool gameEnded = false;
	static private bool generateSpecialRandom = false;
	private string specialType = "";
	private bool initialized    = false;
	private int score = 0;
	static private int prepareScore = 0;
	public float totalTime;
	
	private int columns = 7;
	private int rows = 6;
	
	private CubeDefine[,] cubeUsing = new CubeDefine[7, 6];
	private List<Vector2> selectedCubes = new List<Vector2>();
	private Vector2 lastSelectedPosition = Vector2.zero;
	private string lastSelectedType = "";

	
	static public Manager Instance;
	public tk2dClippedSprite cubeSprite;
	public tk2dClippedSprite bulletSprite;
	public tk2dSprite berrySprite;
	public tk2dTextMesh movingScore;
	
	public Camera gameCam;
	
	public bool GameEnded
	{
		get {return gameEnded;}
		set {gameEnded = value;}
	}
	
	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
		}
	}
	
	// Use this for initialization
	void Awake () 
	{
		Instance = this;	
	}
	
	void Start() 
	{
		StartCoroutine(MainLoop());
	}
	
	private IEnumerator MainLoop() 
	{
		
		while (! gameEnded) 
		{
			for (int i = 0; i < columns; i++) 
			{
				StartCoroutine(InitialCubes(i));
				StartCoroutine(MoveCubes(i));
				StartCoroutine(NewCubes(i));
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator InitialCubes(int current_column) 
	{
		
		if (! initialized) 
		{
			for (int current_row = 0; current_row < rows; current_row++) 
			{
				CubeDefine cube = new CubeDefine();
				string cubeType;
				tk2dClippedSprite sprite;
				
				sprite = Instantiate(cubeSprite, Vector3.zero , Quaternion.identity) as tk2dClippedSprite;
				CubeType.normalType.TryGetValue(Random.Range(1, 6), out cubeType);
				sprite.SetSprite(cubeType + "_normal");
				
				cube.CubeObject = sprite.gameObject;
				cube.Type = cubeType;
				cube.cubeScript.SetPosition(current_column, current_row);
				cube.cubeScript.MoveTo(current_column, current_row);
				cubeUsing[current_column, current_row] = cube;
			}
			if (current_column == columns - 1) 
			{
				initialized = true;
				yield break;
			} 
			else 
			{
				yield return null;
			}
		}
	}
	
	private IEnumerator MoveCubes(int current_column) 
	{
		
		if (initialized) 
		{
			for (int current_row = 0; current_row < rows; current_row++) 
			{
				if (cubeUsing[current_column, current_row] == null && (current_row < rows - 1)) 
				{
					int i = 0;
					while (cubeUsing[current_column, current_row + i] == null)
					{
						i++;
						if (current_row + i == rows)
						{
							break;
						}
					}
					if (current_row + i < rows)
					{
						cubeUsing[current_column, current_row + i].cubeScript.MoveTo(current_column, current_row);
						cubeUsing[current_column, current_row] = cubeUsing[current_column, current_row + i];
						cubeUsing[current_column, current_row + i] = null;
					}
				}
			}
			yield return null;
		} 
		else 
		{
			yield break;
		}
	}
	
	private IEnumerator NewCubes(int current_column) 
	{
		if (initialized) 
		{
			for (int current_row =0; current_row < rows; current_row++) 
			{
				if (cubeUsing[current_column, current_row] == null) 
				{
					CubeDefine cube = new CubeDefine();
					string cubeType;
					tk2dClippedSprite sprite;
					
					sprite = Instantiate(cubeSprite, Vector3.zero , Quaternion.identity) as tk2dClippedSprite;
					CubeType.normalType.TryGetValue(Random.Range(1, 6), out cubeType);
					sprite.SetSprite(cubeType + "_normal");
					cube.Type = cubeType;
					cube.CubeObject = sprite.gameObject;
					cube.cubeScript.SetPosition(current_column, current_row);
					cube.cubeScript.MoveTo(current_column, current_row);
					cubeUsing[current_column, current_row] = cube;
				}
			}
			yield return null;
		} 
		else 
		{
			yield break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameEnded)
		{
			return;
		}
		if (Input.touchCount > 0) 
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = gameCam.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(ray, out hit))
			{
				for (int i = 0; i < columns; i++)
				{
					for (int j = 0; j < rows; j++)
					{
						if (cubeUsing[i, j] != null && (cubeUsing[i, j].cubeScript.ColliderTransform == hit.transform)) 
						{
							
							string cubeType = cubeUsing[i, j].Type;
							
							Vector2 currentCubeVector = new Vector2(i, j);
							
							if (Input.GetTouch(0).phase == TouchPhase.Began)
							{
								if (CubeType.normalType.ContainsValue(cubeType))
								{
									cubeUsing[i, j].cubeScript.sprite.SetSprite(cubeType + "_active");
									selectedCubes.Add(currentCubeVector);
									lastSelectedPosition = currentCubeVector;
									lastSelectedType = cubeType;
								}
								else if (cubeType == "special_fire")
								{
									SpecialFire(currentCubeVector);
								}
								else if (cubeType == "special_random")
								{
									SpecialRandom(currentCubeVector);
								}
							}
							
							else if (Input.GetTouch(0).phase == TouchPhase.Moved)
							{
								if (! selectedCubes.Contains(currentCubeVector))
								{										
									if (
										( 
											( lastSelectedPosition.x % 2 == 0
											&& (  (Mathf.Abs(currentCubeVector.x - lastSelectedPosition.x) == 1 && (lastSelectedPosition.y - currentCubeVector.y == 0 
																																				|| lastSelectedPosition.y - currentCubeVector.y == 1)
												   )
												|| (currentCubeVector.x == lastSelectedPosition.x && (Mathf.Abs(currentCubeVector.y - lastSelectedPosition.y) == 1))
												   )
												)
										   )
										   || 
										   ( lastSelectedPosition.x % 2 == 1
											&& (  (Mathf.Abs(currentCubeVector.x - lastSelectedPosition.x) == 1 && (currentCubeVector.y - lastSelectedPosition.y == 0 
																																				|| currentCubeVector.y - lastSelectedPosition.y == 1)
													)
												|| (currentCubeVector.x == lastSelectedPosition.x && (Mathf.Abs(currentCubeVector.y - lastSelectedPosition.y) == 1)
													)
												)
										   )
										)
									{
										if (cubeType == lastSelectedType)
										{
											cubeUsing[i, j].cubeScript.sprite.SetSprite(cubeType + "_active");
											selectedCubes.Add(currentCubeVector);
											lastSelectedPosition = currentCubeVector;
											lastSelectedType = cubeType;
											SpawnScore();
											SpawnBerry();
										}
									}
								}
							}
							break;
						}
					}
				}
			}
			if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
			{
				if (selectedCubes.Count < 3 && selectedCubes.Count > 0)
				{
					foreach (Vector2 selected in selectedCubes)
					{
						if (
							(cubeUsing[(int)selected.x, (int)selected.y].Type != "special_random")
							&& (cubeUsing[(int)selected.x, (int)selected.y].Type != "special_fire")
							&& (cubeUsing[(int)selected.x, (int)selected.y].Type != "special_rabbit")
						) 
						{
							cubeUsing[(int)selected.x, (int)selected.y].cubeScript.sprite.SetSprite(lastSelectedType + "_normal");
						}
					}
				}
				else if (selectedCubes.Count >= 3) 
				{
					if (specialType == "")
					{
						ReleaseBerry();
						IncreaseSpecialProgress();
						SpawnSpecialCube();
					}
					SpawnBullet();
					CauculateScore();
					DestroyCube();
				}
				RefreshTouch();
			}
		}
	}
	
	private void RefreshTouch()
	{
		specialType = "";
		lastSelectedType = "";
		lastSelectedPosition = Vector2.zero;
		selectedCubes.Clear();
	}
	
	private void SpawnBullet()
	{
		List<Vector2>.Enumerator e = selectedCubes.GetEnumerator();
		Vector3 enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform.position;
		int i = 0;
		while (e.MoveNext())
		{
			if (i >= 2)
			{
				Vector2 currentSelected = e.Current;
				Vector3 spawnPosition = new Vector3(
					cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.x,
					cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.y,
					bulletSprite.transform.position.z
				);
				tk2dClippedSprite bullet = Instantiate(bulletSprite, spawnPosition , Quaternion.identity) as tk2dClippedSprite;
				bullet.GetComponent<Bullet>().SetDestination(enemyPosition);
			}
			i++;
		}
	}
	
	private void SpawnBerry()
	{
		if (selectedCubes.Count >= 4)
		{
			Vector2 currentSelected = selectedCubes[selectedCubes.Count - 1];
			Vector3 spawnPosition = new Vector3(
				cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.x,
				cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.y,
				bulletSprite.transform.position.z
			);
			Instantiate(berrySprite, spawnPosition, Quaternion.identity);
		}
	}
	
	private void ReleaseBerry()
	{
		GameObject receiver = GameObject.FindGameObjectWithTag("BerryReceiver");
		GameObject[] berries;
		berries = GameObject.FindGameObjectsWithTag("Berry");
		foreach (GameObject berry in berries)
		{
			berry.GetComponent<Berry>().SetDestination(receiver.transform.position);
		}
	}
	
	private void SpecialRandom(Vector2 position)
	{
		string destroyType = "";
		CubeType.normalType.TryGetValue(Random.Range(1, 6), out destroyType);
		
		selectedCubes.Add(position);
		for (int i = 0; i < columns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				if (cubeUsing[i, j].Type == destroyType)
				{
					selectedCubes.Add(new Vector2(i, j));
				}
			}
		}
		specialType = "special_random";
		prepareScore += 8000;
	}
	
	private void SpecialFire(Vector2 position)
	{
		int x = (int)position.x;
		int y = (int)position.y;
		List<Vector2> fireCube;
		if (x % 2 == 0)
		{
			fireCube = new List<Vector2>()
			{
				new Vector2(x, y), new Vector2(x, y + 1), new Vector2(x, y + 2),
				new Vector2(x - 1, y), new Vector2(x - 1, y + 1), new Vector2(x + 1, y), new Vector2(x + 1, y + 1),
				new Vector2(x - 2, y + 1), new Vector2(x - 2, y + 2), new Vector2(x + 2, y + 1), new Vector2(x + 2, y + 2)
			};
		}
		else
		{
			fireCube = new List<Vector2>()
			{
				new Vector2(x, y), new Vector2(x, y + 1), new Vector2(x, y + 2),
				new Vector2(x - 1, y + 1), new Vector2(x - 1, y + 2), new Vector2(x + 1, y + 1), new Vector2(x +1, y + 2),
				new Vector2(x - 2, y + 1), new Vector2(x - 2, y + 2), new Vector2(x + 2, y + 1), new Vector2(x + 2, y + 2)
			};
		}
		if (fireCube.Count > 0)
		{
			foreach (Vector2 selected in fireCube)
			{
				if ((int)selected.x >= 0 && (int)selected.x < columns && (int)selected.y >=0 && (int)selected.y <= rows)
				{
					selectedCubes.Add(selected);
				}
			}
		}
		specialType = "special_fire";
		prepareScore += 10000;
	}
	
	private void SpawnSpecialCube()
	{
		if (generateSpecialRandom)
		{
			int x = Random.Range(0, columns);
			int y = Random.Range(0, rows);
			cubeUsing[x, y].Type = "special_random";
			cubeUsing[x, y].cubeObject.GetComponent<tk2dClippedSprite>().SetSprite("special_random");
			generateSpecialRandom = false;
		}
		
		if (selectedCubes.Count >= 7)
		{
			Vector2 generatePosition = selectedCubes[selectedCubes.Count - 1];
			cubeUsing[(int)generatePosition.x, (int)generatePosition.y].Type = "special_fire";
			cubeUsing[(int)generatePosition.x, (int)generatePosition.y].cubeObject.GetComponent<tk2dClippedSprite>().SetSprite("special_fire");
			selectedCubes.RemoveAt(selectedCubes.Count - 1);
		}
	}
	
	private void DestroyCube()
	{
		foreach (Vector2 selected in selectedCubes)
		{
			Destroy(cubeUsing[(int)selected.x, (int)selected.y].CubeObject);
			cubeUsing[(int)selected.x, (int)selected.y] = null;
		}
	}
	
	private void SpawnScore()
	{
		if (selectedCubes.Count >= 2)
		{
			Vector2 currentSelected = selectedCubes[selectedCubes.Count - 1];
			Vector3 spawnPosition = new Vector3(
				cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.x,
				cubeUsing[(int)currentSelected.x, (int)currentSelected.y].CubeObject.transform.position.y,
				movingScore.transform.position.z
			);
			int plusScore = (int)Mathf.Pow(selectedCubes.Count, 2) * 20 + 120;
			tk2dTextMesh scoreSprite =  Instantiate(movingScore, spawnPosition, Quaternion.identity) as tk2dTextMesh;
			scoreSprite.GetComponent<MovingScore>().Score = plusScore;
			prepareScore += plusScore;
		}
	}
	
	private void CauculateScore()
	{
		if (selectedCubes.Count >= 3)
		{
			score += prepareScore;
		}
		prepareScore = 0;
	}
	
	private void IncreaseSpecialProgress()
	{
		if (selectedCubes.Count >= 3)
		{
			GameObject specialBar = GameObject.FindGameObjectWithTag("SpecialProgress");
			specialBar.GetComponent<Score>().SpecialProgress += Mathf.Pow(selectedCubes.Count, 2) * 0.005f;
			if (specialBar.GetComponent<Score>().SpecialProgress >= 1.0f)
			{
				generateSpecialRandom = true;
				specialBar.GetComponent<Score>().SpecialProgress = 0.5f;
			}
		}
	}
}
