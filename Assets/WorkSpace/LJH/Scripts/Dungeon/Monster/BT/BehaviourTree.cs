public class BehaviourTree
{
    private BTNode root;

    public BehaviourTree(BTNode root)
    {
        this.root = root;
    }

    public void Tick()
    {
        root.Tick();
    }
}

