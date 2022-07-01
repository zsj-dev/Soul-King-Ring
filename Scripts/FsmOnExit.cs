using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmOnExit : StateMachineBehaviour{

    public string[] on_ExitMessages;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (var message in on_ExitMessages) {
            animator.gameObject.SendMessageUpwards(message);
        }
    }

}
