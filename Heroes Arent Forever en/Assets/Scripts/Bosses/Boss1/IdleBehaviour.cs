using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{

   public float timer;
   public float minTime;
   public float maxTime;
   private int rand;   
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      rand = Random.Range(0,6);
      timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(timer <= 0){

            if(rand <= 1){
             animator.SetTrigger("Attack1");
            }else if(rand <= 3){
               animator.SetTrigger("Attack2");
            }else if(rand <= 6){
                animator.SetTrigger("Attack3");
            }
        }
        else
        {
            timer -= Time.deltaTime;
         }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
