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
        public GameObject[] Arts;
        
        private GameObject mPickUpObject;

        [Range(0, 1000)] public int Money = 800;

        private Leaf goToFrontDoor;
        private Leaf goToBackDoor;
        
        new void Start()
        {
            base.Start();
            Sequence steal = new Sequence("Steal Something");
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            
            PSelector openDoor = new PSelector("Open Door");
            goToFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor,1);
            goToBackDoor = new Leaf("Go To Back Door", GoToBackDoor,2);

            RSelector selectObject = new RSelector("Select Object to Steal");
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond,2);
            Leaf goToPainting = new Leaf("Go To Diamond", GoToPainting,1);

            Leaf goToArt1 = new Leaf("Go To Art 1", GoToArt1);
            Leaf goToArt2 = new Leaf("Go To Art 2", GoToArt2);
            Leaf goToArt3 = new Leaf("Go To Art 3", GoToArt3);
            // Leaf goToArt4 = new Leaf("Go To Art 4", GoToArt4);
            // Leaf goToArt5 = new Leaf("Go To Art 5", GoToArt5);
            
            Leaf goToVan = new Leaf("Go To Van", GoToVan);

            Inverter inverterMoney = new Inverter("Inverter Money");
            inverterMoney.AddChild(hasGotMoney);

            openDoor.AddChild(goToFrontDoor);
            openDoor.AddChild(goToBackDoor);

            steal.AddChild(inverterMoney);

            steal.AddChild(openDoor);
            steal.AddChild(selectObject);
            selectObject.AddChild(goToArt1);
            selectObject.AddChild(goToArt2);
            selectObject.AddChild(goToArt3);
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
            Node.Status s= GoToDoor(FrontDoor);
            if (s == Node.Status.FAILURE)
                goToFrontDoor.SortOrder = 10;
            else
                goToFrontDoor.SortOrder = 1;
            return s;
        }

        private Node.Status GoToBackDoor()
        {
            Node.Status s= GoToDoor(BackDoor);
            if (s == Node.Status.FAILURE)
                goToBackDoor.SortOrder = 10;
            else
                goToBackDoor.SortOrder = 1;
            return s;
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
        
        private Node.Status GoToArt1()
        {
            if (!Arts[0].activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Arts[0].transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Arts[0].transform.position = transform.position + Vector3.up * 2;
                Arts[0].transform.SetParent(transform);
                mPickUpObject = Arts[0];
            }
            return status;
        }
        
        private Node.Status GoToArt2()
        {
            if (!Arts[1].activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Arts[1].transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Arts[1].transform.position = transform.position + Vector3.up * 2;
                Arts[1].transform.SetParent(transform);
                mPickUpObject = Arts[1];
            }
            return status;
        }
        
        private Node.Status GoToArt3()
        {
            if (!Arts[2].activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Arts[2].transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Arts[2].transform.position = transform.position + Vector3.up * 2;
                Arts[2].transform.SetParent(transform);
                mPickUpObject = Arts[2];
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