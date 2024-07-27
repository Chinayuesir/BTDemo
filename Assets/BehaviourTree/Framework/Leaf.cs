using System;

namespace BTKit
{
    public class Leaf : Node
    {
        private Func<Status> mProcessMethod;
        
        public Leaf(string n, Func<Status> processMethod)
        {
            mName = n;
            mProcessMethod = processMethod;
        }

        public override Status Process()
        {
            if(mProcessMethod!=null)
                return mProcessMethod.Invoke();
            return Status.FAILURE;
        }
    }
}
