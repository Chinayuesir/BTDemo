using UnityEngine;
using UnityEngine.AI;

namespace BTKit.Demo
{
    public class RobberBehaviour : BTAgent
    {
        public GameObject FrontDoor;
        public GameObject BackDoor;
        public GameObject Diamond;
        public GameObject Painting;
        public GameObject Van;

        private GameObject mPickUpObject;

        [Range(0, 1000)] public int Money = 800;

        new void Start()
        {
            base.Start();
            Sequence steal = new Sequence("Steal Something");
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            
            Selector openDoor = new Selector("Open Door");
            Leaf goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor);

            Selector selectObject = new Selector("Select Object to Steal");
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
            Leaf goToPainting = new Leaf("Go To Diamond", GoToPainting);
            
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            Inverter inverterMoney = new Inverter("Inverter Money");
            inverterMoney.AddChild(hasGotMoney);

            openDoor.AddChild(goToFrontDoor);
            openDoor.AddChild(goToBackDoor);

            steal.AddChild(inverterMoney);

            steal.AddChild(openDoor);
            steal.AddChild(selectObject);
            selectObject.AddChild(goToDiamond);
            selectObject.AddChild(goToPainting);
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
            if (!Diamond.activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Diamond.transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Diamond.transform.position = transform.position + Vector3.up * 2;
                Diamond.transform.SetParent(transform);
                mPickUpObject = Diamond;
            }
            return status;
        }
        
        private Node.Status GoToPainting()
        {
            if (!Painting.activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Painting.transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Painting.transform.position = transform.position + Vector3.up * 2;
                Painting.transform.SetParent(transform);
                mPickUpObject = Painting;
            }
            return status;
        }

        private Node.Status GoToVan()
        {
            var status = GoToLocation(Van.transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Money += 300;
                mPickUpObject.SetActive(false);
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
                    door.GetComponent<NavMeshObstacle>().enabled = false;
                    return Node.Status.SUCCESS;
                }

                return Node.Status.FAILURE;
            }

            return status;
        }
    }
}