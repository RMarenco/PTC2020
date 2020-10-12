using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureGen : MonoBehaviour
{
	public GameObject[] RowenaTreasure;
    public GameObject[] AsusTreasure;
    public GameObject[] OtherTreasure;
    GameObject AttackTreasureGO;
    GameObject OtherTreasureGO;
	int AttackTreasureElement;
    int OtherTreasureElement;

    // Start is called before the first frame update
    void Start()
    {
        if(RowenaTreasure.Length != 0 && AsusTreasure.Length != 0){
            if(EventManager.SelectedCharacter == "Rowena"){
                AttackTreasureElement = Random.Range(0, RowenaTreasure.Length);
                AttackTreasureGO = Instantiate(RowenaTreasure[AttackTreasureElement], this.transform.position, Quaternion.identity);
            }else{
                AttackTreasureElement = Random.Range(0, AsusTreasure.Length);
                AttackTreasureGO = Instantiate(AsusTreasure[AttackTreasureElement], this.transform.position, Quaternion.identity);
            }                
            AttackTreasureGO.transform.parent = gameObject.transform;    
        }else{
            if(EventManager.SelectedCharacter == "Rowena"){
            
                OtherTreasureGO = Instantiate(OtherTreasure[0], this.transform.position, Quaternion.identity);
            }   
            else{
                OtherTreasureGO = Instantiate(OtherTreasure[1], this.transform.position, Quaternion.identity);
            }
            OtherTreasureGO.transform.parent = gameObject.transform;            
        }
    	
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
