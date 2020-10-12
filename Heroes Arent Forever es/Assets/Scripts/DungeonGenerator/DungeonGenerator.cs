using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;
    public string[] roomNames;
    int myElements;

    private void Start(){
    	dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
    	SpawnRooms(dungeonRooms);        
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms){

    	RoomController.instance.LoadRoom("Start", 0, 0);
    	foreach(Vector2Int roomLocation in rooms){   
            myElements = Random.Range(0, roomNames.Length); 		    		
            RoomController.instance.LoadRoom(roomNames[myElements], roomLocation.x, roomLocation.y);        
    	}
    }
}
