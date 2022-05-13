using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimBehavior : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    //StaffController.isStaffInHitt = true;
    // StaffController.SetStaffCollEnabled(true);
    //StaffController.MoveForward();
    EventsManager.TriggerEvent(EventsIds.STAFF_START_HIT);
  }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    //StaffController.isStaffInHitt = false;
    //StaffController.SetStaffCollEnabled(false);
    //StaffController.MoveBack();
    EventsManager.TriggerEvent(EventsIds.STAFF_END_HIT);
    //Debug.Log("Выходим из удара");
  }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
