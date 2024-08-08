using BTKit.Demo.Utils;

namespace BTKit
{
    public class RSelector:Node
    {
        private bool mShuffled = false;
        public RSelector(string n)
        {
            mName = n;
        }
    
        public override Status Process()
        {
            if (!mShuffled)
            {
                Children.Shuffle();
                mShuffled = true;
            }
            Status childStatus = Children[mCurrentChild].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.SUCCESS)
            {
                mCurrentChild = 0;
                mShuffled = false;
                return Status.SUCCESS;
            }
            mCurrentChild++;
            if (mCurrentChild >= Children.Count)
            {
                mCurrentChild = 0;
                mShuffled = false;
                return Status.FAILURE;
            }
            return Status.RUNNING;
        }
    }
}
