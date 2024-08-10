namespace BTKit.Demo
{
    public class Loop:Node
    {
        private BehaviourTree mDependancy;
      
        public Loop(string n,BehaviourTree d)
        {
            mName = n;
            mDependancy = d;
        }

        public override Status Process()
        {
            if (mDependancy.Process() == Status.FAILURE)
            {
                return Status.SUCCESS;
            }

            Status childStatus = Children[mCurrentChild].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.FAILURE) return childStatus;

            mCurrentChild++;
            if (mCurrentChild >= Children.Count)
            {
                mCurrentChild = 0;
            }

            return Status.RUNNING;
        }
    }
}