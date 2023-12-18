public class OneNodeScenario : Scenario
{
    protected override void load(Model model, int nodeCount)
    {
        Node main = new Node();
        main.Type = ProcessType.Mixed;
        main.Size = NodeSize.Medium;

        main.MainChildren.Add(main);
        model.Add(main);
    }
}