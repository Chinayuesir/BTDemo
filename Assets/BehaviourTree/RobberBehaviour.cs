using UnityEngine;
using UnityEngine.AI;

namespace BTKit.Demo
{
    public class RobberBehaviour : MonoBehaviour
{
    private BehaviourTree mTree;
    public GameObject FrontDoor;
    public GameObject BackDoor;
    public GameObject Diamond;
    public GameObject Van;

    private NavMeshAgent mAgent;

    public enum ActionState
    {
        IDLE,
        WORKING
    }

    private ActionState mState = ActionState.IDLE;
    private Node.Status mTreeStatus = Node.Status.RUNNING;

    [Range(0, 1000)] public int Money=800;

    private void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();

        mTree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Selector openDoor = new Selector("Open Door");
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
        Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        
        steal.AddChild(hasGotMoney);

        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        mTree.AddChild(steal);

        mTree.PrintTree();
    }
    
    private Node.Status HasMoney()
    {
        if (Money > 500) return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    private Node.Status GoToFrontDoor()
    {
        return GoToDoor(FrontDoor);
    }

    private Node.Status GoToBackDoor()
    {
        return GoToDoor(BackDoor);
    }

    private Node.Status GoToDiamond()
    {
        var status = GoToLocation(Diamond.transform.position);
        if (status == Node.Status.SUCCESS)
            Diamond.transform.parent = transform;
        return status;
    }

    private Node.Status GoToVan()
    {
        var status = GoToLocation(Van.transform.position);
        if (status == Node.Status.SUCCESS)
        {
            Money += 300;
            Diamond.SetActive(false);
        }
        return status;
    }

    private Node.Status GoToDoor(GameObject door)
    {
        Node.Status status = GoToLocation(door.transform.position);
        if (status == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().IsLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        return status;
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);
        if (mState == ActionState.IDLE)
        {
            mAgent.SetDestination(destination);
            mState = ActionState.WORKING;
        }
        else if (Vector3.Distance(mAgent.pathEndPosition, destination) >= 2)
        {
            mState = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            mState = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    private void Update()
    {
        if (mTreeStatus != Node.Status.SUCCESS)
            mTreeStatus = mTree.Process();
    }
}
}
