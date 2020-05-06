using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuUnity.Samples
{
    public class TofuStateMachineSample : TofuStateMachine
    {
        public GameObject stateTransitionRepresentation;

        private void Start()
        {
            // iterate through each to create a transition between each permutation
            foreach (MonoBehaviour from in _states)
            {
                foreach (MonoBehaviour to in _states)
                {
                    Vector3 startPos = from.transform.position;
                    Vector3 endPos = to.transform.position;

                    RegisterTransition(from.name, to.name, () =>
                    {
                        // animate the transition representation to the next state
                        stateTransitionRepresentation.Sequence()
                            .Curve(TofuAnimator.EEaseType.EaseOutExpo, 1f, (float percent) =>
                            {
                                stateTransitionRepresentation.transform.position = Vector3.LerpUnclamped(startPos, endPos, percent);
                            })
                            .Play();
                    });
                }
            }
        }

        public void GoToStateA()
        {
            TransitionTo<TofuStateMachineSample_State_A>();
        }

        public void GoToStateB()
        {
            TransitionTo<TofuStateMachineSample_State_B>();
        }

        public void GoToStateC()
        {
            TransitionTo<TofuStateMachineSample_State_C>();
        }

        public void GoToStateD()
        {
            TransitionTo<TofuStateMachineSample_State_D>();
        }
    }
}