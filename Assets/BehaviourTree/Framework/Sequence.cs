namespace BTKit
{
    public class Sequence:Node
    {
        public Sequence(string n)
        {
            mName = n;
        }

        public override Status Process()
        {
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
