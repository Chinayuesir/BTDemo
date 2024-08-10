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
        public GameObject Cop;

        public GameObject[] Arts;

        private GameObject mPickUpObject;
        private Vector3 remeberedLocation;

        [Range(0, 1000)] public int Money = 800;

        private Leaf goToFrontDoor;
        private Leaf goToBackDoor;

        new void Start()
        {
            base.Start();
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
            Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

            RSelector selectObject = new RSelector("Select Object to Steal");
            for (int i = 0; i < Arts.Length; i++)
            {
                Leaf gta = new Leaf("Go to " + Arts[i].name, GoToArt, i);
                selectObject.AddChild(gta);
            }

            goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
            goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);
            PSelector opendoor = new PSelector("Open Door");

            Sequence runAway = new Sequence("Run Away");
            Leaf canSee = new Leaf("Can See Cop?", CanSeeCop);
            Leaf flee = new Leaf("Flee From Cop", FleeFromCop);

            Inverter invertMoney = new Inverter("Invert Money");
            invertMoney.AddChild(hasGotMoney);

            opendoor.AddChild(goToFrontDoor);
            opendoor.AddChild(goToBackDoor);

            runAway.AddChild(canSee);
            runAway.AddChild(flee);

            Inverter cantSeeCop = new Inverter("Cant See Cop");
            cantSeeCop.AddChild(canSee);

            BehaviourTree stealConditions = new BehaviourTree("stealConditions");
            Sequence conditions = new Sequence("Conditions");
            conditions.AddChild(cantSeeCop);
            conditions.AddChild(invertMoney);
            stealConditions.AddChild(conditions);
            DepSequence steal = new DepSequence("Steal Something",stealConditions,mAgent);
            
            steal.AddChild(opendoor);
            steal.AddChild(selectObject);
            steal.AddChild(goToVan);

            Selector stealWithFallback = new Selector("Steal with Fallback");
            stealWithFallback.AddChild(steal);
            stealWithFallback.AddChild(goToVan);

            Selector beThief = new Selector("Be a thief");
            beThief.AddChild(stealWithFallback);
            beThief.AddChild(runAway);

            mTree.AddChild(beThief);

            mTree.PrintTree();
        }

        private Node.Status CanSeeCop()
        {
            return CanSee(Cop.transform.position, "Cop", 10, 90);
        }

        private Node.Status FleeFromCop()
        {
            return Flee(Cop.transform.position, 10);
        }

        private Node.Status HasMoney()
        {
            if (Money < 500) return Node.Status.FAILURE;
            return Node.Status.SUCCESS;
        }

        private Node.Status GoToFrontDoor()
        {
            Node.Status s = GoToDoor(FrontDoor);
            if (s == Node.Status.FAILURE)
                goToFrontDoor.SortOrder = 10;
            else
                goToFrontDoor.SortOrder = 1;
            return s;
        }

        private Node.Status GoToBackDoor()
        {
            Node.Status s = GoToDoor(BackDoor);
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

        private Node.Status GoToArt(int i)
        {
            if (!Arts[i].activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Arts[i].transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Arts[i].transform.position = transform.position + Vector3.up * 2;
                Arts[i].transform.SetParent(transform);
                mPickUpObject = Arts[i];
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
            if (status == Node.Status.SUCCESS && mPickUpObject!=null)
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

        private Node.Status CanSee(Vector3 target, string tag, float distance, float maxAngle)
        {
            Vector3 dirToTarget = target - transform.position;
            float angle = Vector3.Angle(dirToTarget, transform.forward);
            if (angle <= maxAngle && dirToTarget.magnitude <= distance)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, dirToTarget, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.CompareTag(tag))
                    {
                        return Node.Status.SUCCESS;
                    }
                }
            }

            return Node.Status.FAILURE;
        }

        private Node.Status Flee(Vector3 location, float distance)
        {
            if (mState == ActionState.IDLE)
            {
                remeberedLocation = transform.position + (transform.position - location).normalized * distance;
            }

            return GoToLocation(remeberedLocation);
        }
    }
}