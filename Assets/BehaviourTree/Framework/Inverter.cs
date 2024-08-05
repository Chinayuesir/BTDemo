namespace BTKit
{
    /// <summary>
    /// One child
    /// </summary>
    public class Inverter:Node
    {
        public Inverter(string n)
        {
            mName = n;
        }

        public override Status Process()
        {
            Status childStatus = Children[0].Process();
            if(childStatus==Status.RUNNING) return Status.RUNNING;
            if (childStatus == Status.FAILURE) return Status.SUCCESS;
            else return Status.FAILURE;
        }
    }
}
