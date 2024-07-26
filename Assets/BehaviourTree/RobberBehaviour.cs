using System;
using UnityEngine;


public class RobberBehaviour : MonoBehaviour
{
    private BehaviourTree mTree;
    private void Start()
    {
        mTree = new BehaviourTree();
        Node steal = new Node("Steal Something");
        Node goToDiamond = new Node("Go To Diamond");
        Node goToVan = new Node("Go To Van");
        
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        mTree.AddChild(steal);

        Node eat = new Node("Eat Something");
        Node pizza = new Node("Go To Pizza Shop");
        Node buy = new Node("Buy Pizza");
        
        eat.AddChild(pizza);
        eat.AddChild(buy);
        mTree.AddChild(eat);
        
        mTree.PrintTree();
    }
}