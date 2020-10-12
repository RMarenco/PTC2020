using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class RoomInfo
{
	public string name;

	public int X;

	public int Y;
}

public class RoomController : MonoBehaviour
{
	public static RoomController instance;
	
	public GameObject Dots;
	public Image Blades;
	public Text loadingText;
	public GameObject BlackImage;
	public Text[] Loading_Texts;

	public string currentWorldName;

	RoomInfo currentLoadRoomData;

	float progress;

	Room currRoom;

	Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

	public List<Room> loadedRooms = new List<Room>();

	bool isLoadingRoom = false;
	public bool spawnedBossRoom = false;
	public bool spawnedTreasureRoom = false;
	bool updatedRooms = false;

	void Awake()
	{
		Time.timeScale = 0;		
		instance = this;
	}

	void Start(){
		// LoadRoom("Start", 0, 0);
		// LoadRoom("Empty", 1, 0);
		// LoadRoom("Empty", -1, 0);
		// LoadRoom("Empty", 0, 1);
		// LoadRoom("Empty", 0, -1);
	}

	void Update(){
		UpdateRoomQueue();

	}

	void UpdateRoomQueue(){
		if(isLoadingRoom){
			return;
		}

		if(loadRoomQueue.Count == 0 && !isLoadingRoom){
			
			if(!spawnedBossRoom){
				StartCoroutine(SpawnBossRoom());				
				if(spawnedBossRoom && !spawnedTreasureRoom){
					StartCoroutine(SpawnTreasureRoom());
				}
				if(spawnedTreasureRoom && spawnedBossRoom && !updatedRooms){
					StartCoroutine(RemoveDoorsWait());					
				}
			}			
			return;
		}else if(loadRoomQueue.Count > 0 && !isLoadingRoom){
			
			currentLoadRoomData = loadRoomQueue.Dequeue();
			isLoadingRoom = true;

			StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
		}
	}

	IEnumerator RemoveDoorsWait(){		
		
		yield return new WaitForSecondsRealtime(2);	
		updatedRooms = true;
		Debug.Log("Removing doors");	
		foreach(Room room in loadedRooms){
			room.RemoveUnconnectedDoors();
		}			
		Blades.GetComponent<Animator>().enabled = false;
		Dots.GetComponent<Animator>().enabled = false;
		Color temp = Blades.GetComponent<Image>().color;
	 	temp.a = 0; 				
		Blades.GetComponent<Image>().color = temp;	
		foreach(Text LoadingText in Loading_Texts){

			Color tempT = LoadingText.color;
	 		tempT.a -= 10; 	
	 		LoadingText.color = tempT;	
	 		Debug.Log(LoadingText.color);
		}	
		Time.timeScale = 1.0f;		
		//yield return new WaitForSecondsRealtime(1);	
		//BlackImage.SetActive(false);			
	}

	IEnumerator SpawnBossRoom(){
		spawnedBossRoom = true;	
		yield return new WaitForSeconds(0f);
		Debug.Log("Boss Room: "+loadRoomQueue.Count);
		if(loadRoomQueue.Count == 0){
			Room bossRoom = loadedRooms[loadedRooms.Count - 1];
			Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
			Destroy(bossRoom.gameObject);
			var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
			loadedRooms.Remove(roomToRemove);
			LoadRoom("End", tempRoom.X, tempRoom.Y);
		}
	}

	IEnumerator SpawnTreasureRoom(){
		spawnedTreasureRoom = true;
		yield return new WaitForSeconds(0f);
		Debug.Log("Treasure Room: "+loadRoomQueue.Count);
		if(loadRoomQueue.Count == 1){
			Room treasureRoom = loadedRooms[loadedRooms.Count - 1];
			Room tempRoom = new Room(treasureRoom.X, treasureRoom.Y);
			Destroy(treasureRoom.gameObject);
			var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
			loadedRooms.Remove(roomToRemove);
			LoadRoom("Treasure", tempRoom.X, tempRoom.Y);
		}
	}

	public void LoadRoom(string name, int x, int y){
		if(DoesRoomExists(x,y)){
			return;
		}

		RoomInfo newRoomData = new RoomInfo();
		newRoomData.name = name;
		newRoomData.X = x;
		newRoomData.Y = y;

		loadRoomQueue.Enqueue(newRoomData);		
	}

	IEnumerator LoadRoomRoutine(RoomInfo info){
		string roomName = currentWorldName + info.name;

		AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

		//Debug.Log("Loading Room:" + roomName);

		while(loadRoom.isDone == false){			
			yield return null;
		}
	}

	public void RegisterRoom(Room room){
		if(!DoesRoomExists(currentLoadRoomData.X, currentLoadRoomData.Y)){
			room.transform.position = new Vector3(
				currentLoadRoomData.X * room.Width,
				currentLoadRoomData.Y * room.Height,
				0
			);
		
			room.X = currentLoadRoomData.X;
			room.Y = currentLoadRoomData.Y;
			room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
			room.transform.parent = transform;

			isLoadingRoom = false;

			if(loadedRooms.Count == 0){
				CameraController.instance.currRoom = room;
			}

			loadedRooms.Add(room);	
		}else{
			Destroy(room.gameObject);
			isLoadingRoom = false;
		}
	}
    
    public bool DoesRoomExists(int x, int y)
    {
    	return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
    	return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room){
    	CameraController.instance.currRoom = room;
    	currRoom = room;   
    }    
}
