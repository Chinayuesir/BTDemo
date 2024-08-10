using System;
using UnityEngine;

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

        public Leaf(string n, Func<int, Status> processMethodM, int i)
        {
            mName = n;
            mProcessMethodM = processMethodM;
            index = i;
        }

        public Leaf(string n, Func<Status> processMethod, int order)
        {
            mName = n;
            mProcessMethod = processMethod;
            SortOrder = order;
        }

        public override Status Process()
        {
            Status s;
            if (mProcessMethod != null)
                s = mProcessMethod.Invoke();
            else if (mProcessMethodM != null)
                s = mProcessMethodM.Invoke(index);
            else
                s = Status.FAILURE;
            Debug.Log(mName+" "+s);
            return s;
        }
    }
}