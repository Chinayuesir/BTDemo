using System;

namespace BTKit
{
    public class Leaf : Node
    {
        private Func<Status> mProcessMethod;
        private Func<int, Status> mProcessMethodM;

        public int index;
        public Leaf(string n, Func<Status> processMethod)
        {
            mName = n;
            mProcessMethod = processMethod;
        }
        
        public Leaf(string n, Func<int,Status> processMethodM,int i)
        {
            mName = n;
            mProcessMethodM = processMethodM;
            index = i;
        }
        
        public Leaf(string n, Func<Status> processMethod,int order)
        {
            mName = n;
            mProcessMethod = processMethod;
            SortOrder = order;
        }

        public override Status Process()
        {
            if(mProcessMethod!=null)
                return mProcessMethod.Invoke();
            else if(mProcessMethodM!=null)
                return mProcessMethodM.Invoke(index);
            return Status.FAILURE;
        }
    }
}
