using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status
    {
        SUCCESS,
        RUNNING,
        FAILURE
    };
    public Status CurrentStatus;
    public List<Node> Children = new List<Node>();
    public int CurrentChild;
    public string Name;

    public Node()
    {
    }

    public Node(string n)
    {
        Name = n;
    }

    public void AddChild(Node n)
    {  
        Children.Add(n);
    }
}
