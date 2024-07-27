using System.Collections.Generic;

namespace BTKit
{
    public abstract class Node
    {
        public enum Status
        {
            SUCCESS,
            RUNNING,
            FAILURE
        };
        public List<Node> Children = new List<Node>();
        protected int mCurrentChild;
        protected string mName;

        public string Name
        {
            get => mName;
            set => mName = value;
        }
        
        public Node()
        {
        }

        public Node(string n)
        {
            mName = n;
        }

        public abstract Status Process();

        public void AddChild(Node n)
        {  
            Children.Add(n);
        }
    }
}

