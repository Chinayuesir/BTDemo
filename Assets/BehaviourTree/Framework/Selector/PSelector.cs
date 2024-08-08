using System.Collections.Generic;
using System.Linq;

namespace BTKit
{
    public class PSelector:Node
    {
        private bool mOrdered = false;
        public PSelector(string n)
        {
            mName = n;
        }

        void OrderNodes()
        {
            Children = Children.OrderBy(node => node.SortOrder).ToList();
        }

        public override Status Process()
        {
            if (!mOrdered)
            {
                OrderNodes();
                mOrdered = true;
            }
            
            Status childStatus = Children[mCurrentChild].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.SUCCESS)
            {
                mCurrentChild = 0;
                mOrdered = false;
                return Status.SUCCESS;
            }

            mCurrentChild++;
            if (mCurrentChild >= Children.Count)
            {
                mCurrentChild = 0;
                mOrdered = false;
                return Status.FAILURE;
            }
            return Status.RUNNING;
        }
    }
}
