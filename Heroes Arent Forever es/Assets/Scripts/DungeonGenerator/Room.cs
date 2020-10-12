using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{	    
    public Transform enemiesGO;

    public List<GameObject> enemiesInRoom = new List<GameObject>();

	public float Width;

	public int Height;

	public int X;

	public int Y;

    public Room(int x, int y){
        X = x;
        Y = y;
    }

    public Door leftDoor;

    public Door rightDoor;

    public Door topDoor;

    public Door bottomDoor;    

    public List<Door> doors = new List<Door>();
    public List<Door> doorsActive = new List<Door>();
    
    void Start()
    {        
        if(RoomController.instance == null)
        {
        	Debug.Log("You pressed play in the wrong scene!");
        	return;
        }

        Door[] ds = GetComponentsInChildren<Door>();        

        foreach(Door d in ds){
            doors.Add(d);
            switch(d.doorType){
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;                
            }
        }

        RoomController.instance.RegisterRoom(this);

        enemiesGO = gameObject.transform.Find("Enemies");

        if(enemiesGO != null){
            foreach (Transform child in enemiesGO.transform)
            {
                if (child.tag == "Enemy")
                    enemiesInRoom.Add(child.gameObject);
            }
        }
    }

    public void RemoveUnconnectedDoors(){
        foreach(Door door in doors){            
            switch(door.doorType){
                case Door.DoorType.right:
                    if(GetRight() == null)
                        door.gameObject.SetActive(false);
                    else if(GetRight() != null)
                        doorsActive.Add(door);
                    break;
                case Door.DoorType.left:
                    if(GetLeft() == null)
                        door.gameObject.SetActive(false);
                    else if(GetLeft() != null)
                        doorsActive.Add(door);
                    break;
                case Door.DoorType.top:
                    if(GetTop() == null)
                        door.gameObject.SetActive(false);
                    else if(GetTop() != null)
                        doorsActive.Add(door);
                    break;
                case Door.DoorType.bottom:
                    if(GetBottom() == null)
                        door.gameObject.SetActive(false);
                    else if(GetBottom() != null)
                        doorsActive.Add(door);
                    break;  
            }
        }
    }

    public Room GetRight(){
        if(RoomController.instance.DoesRoomExists(X + 1, Y)){
            return RoomController.instance.FindRoom(X + 1, Y);
        }else{
            return null;
        }
    }

    public Room GetLeft(){
        if(RoomController.instance.DoesRoomExists(X - 1, Y)){
            return RoomController.instance.FindRoom(X - 1, Y);
        }else{
            return null;
        }
    }

    public Room GetTop(){
        if(RoomController.instance.DoesRoomExists(X, Y + 1)){
            return RoomController.instance.FindRoom(X, Y + 1);
        }else{
            return null;
        }
    }

    public Room GetBottom(){
        if(RoomController.instance.DoesRoomExists(X, Y - 1)){
            return RoomController.instance.FindRoom(X, Y - 1);
        }else{
            return null;
        }
    }

    void OnDrawGizmos(){
    	Gizmos.color = Color.red;
    	Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height,0));
    }

    public Vector3 GetRoomCentre(){
    	return new Vector3(X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name == "DoorColliders"){
            RoomController.instance.OnPlayerEnterRoom(this);
            
            foreach (GameObject enemiesinRoom in enemiesInRoom){
                if(enemiesinRoom.GetComponent<Enemies>()!=null){
                    enemiesinRoom.GetComponent<Enemies>().playerInRoom = true;                
                    enemiesinRoom.GetComponent<Enemies>().RoomName = this.name;                                
                    Debug.Log("habitación: " + this.name);    
                }else if(enemiesinRoom.GetComponent<Boss1>()!=null){
                    enemiesinRoom.GetComponent<Boss1>().playerInRoom = true;
                    enemiesinRoom.GetComponent<Boss1>().RoomName = this.name;
                }
                
            }
            if(enemiesInRoom.Count > 0){
                foreach (Door SetDoorColliderOff in doorsActive){
                    SetDoorColliderOff.GetComponent<Door>().CanOpenDoor = true;
                    SetDoorColliderOff.GetComponent<Collider2D>().enabled = false;
                }
            }
            Debug.Log(enemiesInRoom.Count);            
        }        
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.name == "DoorColliders"){                                    
            if(enemiesInRoom.Count == 0){
                foreach (Door SetDoorColliderOff in doorsActive){
                    SetDoorColliderOff.GetComponent<Door>().CanOpenDoor = false;
                    SetDoorColliderOff.GetComponent<Collider2D>().enabled = true;
                }
            }
            //Debug.Log(enemiesInRoom.Count);            
        }   
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.name == "DoorColliders"){
            foreach (GameObject enemiesinRoom in enemiesInRoom){
                enemiesinRoom.GetComponent<Enemies>().playerInRoom = false;
            }
            //Debug.Log("You left Room" + this);
        }
    }
}
