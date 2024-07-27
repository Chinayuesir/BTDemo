namespace BTKit
{
    public class Selector:Node
    {
        public Selector(string n)
        {
            mName = n;
        }
    
        public override Status Process()
        {
            Status childStatus = Children[mCurrentChild].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.SUCCESS)
            {
                mCurrentChild = 0;
                return Status.SUCCESS;
            }
            mCurrentChild++;
            if (mCurrentChild >= Children.Count)
            {
                mCurrentChild = 0;
                return Status.FAILURE;
            }
            return Status.RUNNING;
        }
    }
}
