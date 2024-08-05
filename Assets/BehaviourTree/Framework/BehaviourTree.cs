using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTKit.Demo
{
    public class BehaviourTree : Node
    {
        public BehaviourTree(string n ="Tree")
        {
            mName = n;
        }

        public override Status Process()
        {
            if (Children.Count == 0) return Status.SUCCESS;
            return Children[mCurrentChild].Process();
        }

        struct NodeLevel
        {
            public int Level;
            public Node Node;
        }

        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> stack=new Stack<NodeLevel>();
            stack.Push(new NodeLevel(){Level = 0,Node = this});
            while (stack.Count!=0)
            {
                var node = stack.Pop();
                treePrintout += new string('-',node.Level) +node.Node.Name + "\n";
                for (var i = node.Node.Children.Count-1; i >=0; i--)
                {
                    stack.Push(new NodeLevel(){Level = node.Level+1,Node = node.Node.Children[i]});
                }
            }
            Debug.Log(treePrintout);
        }
    }

}
