using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
   public BehaviourTree()
   {
      Name = "Tree";
   }

   public BehaviourTree(string n)
   {
      Name = n;
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
