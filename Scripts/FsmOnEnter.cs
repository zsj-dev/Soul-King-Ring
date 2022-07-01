using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmOnEnter : StateMachineBehaviour{

    public string[] on_EnterMessages;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach(var message in on_EnterMessages) {
            animator.gameObject.SendMessageUpwards(message);
        }
    }
}

