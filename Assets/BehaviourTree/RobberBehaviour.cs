using UnityEngine;
using UnityEngine.AI;

namespace BTKit.Demo
{
    public class RobberBehaviour : BTAgent
    {
        public GameObject FrontDoor;
        public GameObject BackDoor;
        public GameObject Diamond;
        public GameObject Van;

        [Range(0, 1000)] public int Money = 800;

        new void Start()
        {
            base.Start();
            Sequence steal = new Sequence("Steal Something");
            Selector openDoor = new Selector("Open Door");
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            Inverter inverterMoney = new Inverter("Inverter Money");
            inverterMoney.AddChild(hasGotMoney);

            openDoor.AddChild(goToFrontDoor);
            openDoor.AddChild(goToBackDoor);

            steal.AddChild(inverterMoney);

            steal.AddChild(openDoor);
            steal.AddChild(goToDiamond);
            steal.AddChild(goToVan);
            mTree.AddChild(steal);

            mTree.PrintTree();
        }

        private Node.Status HasMoney()
        {
            if (Money < 500) return Node.Status.FAILURE;
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
            {
                Diamond.transform.position = transform.position + Vector3.up * 2;
                Diamond.transform.SetParent(transform);
            }

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
    }
}