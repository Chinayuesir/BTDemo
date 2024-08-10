using System;
using System.Collections;
using QFramework;
using UnityEngine;
using UnityEngine.AI;

namespace BTKit.Demo
{
    public enum ActionState
    {
        IDLE,
        WORKING
    }

    public class BTAgent : MonoBehaviour
    {
        protected BehaviourTree mTree;
        protected NavMeshAgent mAgent;
        
        protected ActionState mState = ActionState.IDLE;
        protected Node.Status mTreeStatus = Node.Status.RUNNING;

        protected float mUpdateFrequency=0.1f;

        protected void Start()
        {
            mAgent = GetComponent<NavMeshAgent>();
            mTree = new BehaviourTree();
            ActionKit.Repeat()
                .Delay(mUpdateFrequency)
                .Callback( ()=>mTreeStatus = mTree.Process())
                .Start(this);
        }
        
        protected Node.Status GoToLocation(Vector3 destination)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if (mState == ActionState.IDLE)
            {
                mAgent.SetDestination(destination);
                mState = ActionState.WORKING;
            }
            else if (Vector3.Distance(mAgent.pathEndPosition, destination) >= 2)
            {
                mState = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distanceToTarget <= 2)
            {
                mState = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }

            return Node.Status.RUNNING;
        }
        
        protected Node.Status GoToDoor(GameObject door)
        {
            Node.Status status = GoToLocation(door.transform.position);
            if (status == Node.Status.SUCCESS)
            {
                if (!door.GetComponent<Lock>().IsLocked)
                {
                    door.GetComponent<NavMeshObstacle>().enabled = false;
                    return Node.Status.SUCCESS;
                }

                return Node.Status.FAILURE;
            }

            return status;
        }
    }
}