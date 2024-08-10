using System.Collections;
using QFramework;
using UnityEngine;
using UnityEngine.AI;

namespace BTKit.Demo
{
    public class PatronBehaviour : BTAgent
    {
        public GameObject FrontDoor;
        public GameObject Home;
        public GameObject[] Arts;

        [Range(0, 1000)] public int Boredom = 0;

        private Leaf goToFrontDoor;
        private Leaf goToBackDoor;

        private bool mAtHome = true;

        new void Start()
        {
            base.Start();
            mUpdateFrequency = Random.Range(0.5f, 1f);
            RSelector selectObject = new RSelector("Select Art to View");
            for (int i = 0; i < Arts.Length; i++)
            {
                Leaf gta = new Leaf("Go to " + Arts[i].name, GoToArt, i);
                selectObject.AddChild(gta);
            }

            Leaf goToFrontDoor = new Leaf("Go to Front Door", GoToFrontDoor);
            Leaf goHome = new Leaf("Go Home", GoHome);
            Leaf isBored = new Leaf("Is Bored?", IsBored);

            Sequence viewArt = new Sequence("View Art");
            viewArt.AddChild(isBored);
            viewArt.AddChild(goToFrontDoor);
            
            
            BehaviourTree whileBored = new BehaviourTree();
            whileBored.AddChild(isBored);
            Loop lookAtPaintings = new Loop("Look", whileBored);
            lookAtPaintings.AddChild(selectObject);
            viewArt.AddChild(lookAtPaintings);
            viewArt.AddChild(goHome);

            Selector bePatron = new Selector("Be An Art Patron");
            bePatron.AddChild(viewArt);

            mTree.AddChild(bePatron);

            mTree.PrintTree();

            ActionKit.Repeat()
                .Condition(() => Boredom < 100 && mAtHome)
                .Callback(() =>
                {
                    Boredom = Mathf.Clamp(Boredom + Random.Range(4,20), 0, 1000);
                    if (Boredom >= 100) mAtHome = false;
                })
                .Delay(1)
                .Start(this);
        }

        private Node.Status IsBored()
        {
            if (Boredom < 100)
                return Node.Status.FAILURE;
            else
                return Node.Status.SUCCESS;
        }

        private Node.Status GoHome()
        {
            var status = GoToLocation(Home.transform.position);
            if (status == Node.Status.SUCCESS) mAtHome = true;
            return status;
        }

        private Node.Status GoToArt(int i)
        {
            if (!Arts[i].activeSelf) return Node.Status.FAILURE;
            var status = GoToLocation(Arts[i].transform.position);
            if (status == Node.Status.SUCCESS)
            {
                Boredom = Mathf.Clamp(Boredom - 150, 0, 1000);
            }

            return status;
        }

        private Node.Status GoToFrontDoor()
        {
            Node.Status s = GoToDoor(FrontDoor);
            return s;
        }
    }
}