public class SimpleScenario : Scenario
{
    protected override void load(Model model, int nodeCount)
    {
        Node main = new Node();
        main.IsMain = true;
        main.Type = ProcessType.Mixed;
        main.Size = NodeSize.Medium;

        Node child1 = new Node();
        child1.Type = ProcessType.Blocking;
        child1.Size = NodeSize.Small;

        Node child2 = new Node();
        child2.Type = ProcessType.NonBlocking;
        child2.Size = NodeSize.Large;

        main.MainChildren.Add(main);
        main.TriggerChildren.Add(child1);
        main.TriggerChildren.Add(child2);

        model.Add(main);
    }
}

public class OceanScenario : Scenario
{
    protected override void load(Model model, int nodeCount)
    {
        Node main = new Node();
        main.IsMain = true;
        main.Type = ProcessType.NonBlocking;
        main.Size = NodeSize.Medium;
        main.MainChildren.Add(main);

        for (int i = 0; i < nodeCount; i++)
        {
            Node node = new Node();
            node.Type = ProcessType.Blocking;
            node.Size = NodeSize.Small;
            main.TriggerChildren.Add(node);
        }

        model.Add(main);
    }
}