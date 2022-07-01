using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmOnUpdate : StateMachineBehaviour{


    public string[] on_UpdateMessages;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (var message in on_UpdateMessages) {
            animator.gameObject.SendMessageUpwards(message);
        }
    }

   
}
