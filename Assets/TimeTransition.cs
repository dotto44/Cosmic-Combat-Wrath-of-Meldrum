using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeTransition : StateMachineBehaviour
{

    [SerializeField] TransitionData[] transitions;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.Get
        for (int t = 0; t < transitions.Length; t++)
        {
            if (evaluateTransition(animator, transitions[t]))
            {
                animator.CrossFade(transitions[t].destination, 0, 0, stateInfo.normalizedTime);
                break;
            }
        }
    }

    private bool evaluateTransition(Animator animator, TransitionData transition)
    {
        //check bool values
        for (int b = 0; b < transition.boolNames.Length; b++)
        {
            if (animator.GetBool(transition.boolNames[b]) != transition.boolValues[b])
            {
                return false;
            }
        }

        //check int values
        for (int i = 0; i < transition.intNames.Length; i++)
        {
            if (animator.GetInteger(transition.intNames[i]) != transition.intValues[i] && !transition.notEqual[i] || animator.GetInteger(transition.intNames[i]) == transition.intValues[i] && transition.notEqual[i])
            {
                return false;
            }
        }

        //check float values
        for (int f = 0; f < transition.floatNames.Length; f++)
        {
            if (animator.GetFloat(transition.floatNames[f]) > transition.floatValuesHigh[f]
                || animator.GetFloat(transition.floatNames[f]) < transition.floatValuesLow[f])
            {
                return false;
            }
        }
        return true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
