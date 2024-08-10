using BTKit.Demo;
using UnityEngine.AI;

namespace BTKit
{
    public class DepSequence:Node
    {
        private BehaviourTree mDependancy;
        protected NavMeshAgent mAgent;
        public DepSequence(string n,BehaviourTree d,NavMeshAgent a)
        {
            mName = n;
            mDependancy = d;
            mAgent = a;
        }

        public override Status Process()
        {
            if (mDependancy.Process() == Status.FAILURE)
            {
                mAgent.ResetPath();
                foreach (var child in Children)
                {
                    child.Reset();
                }
                return Status.FAILURE;
            }

            Status childStatus = Children[mCurrentChild].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.FAILURE) return childStatus;

            mCurrentChild++;
            if (mCurrentChild >= Children.Count)
            {
                mCurrentChild = 0;
                return Status.SUCCESS;
            }

            return Status.RUNNING;
        }
    }
}
