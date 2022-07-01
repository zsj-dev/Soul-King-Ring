using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmClearTrigger : StateMachineBehaviour{

    public string[] clear_EnterTrigger;
    public string[] clear_ExitTrigger;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (var trigger in clear_EnterTrigger) {
            animator.ResetTrigger(trigger);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (var trigger in clear_ExitTrigger) {
            animator.ResetTrigger(trigger);
        }
    }
}
