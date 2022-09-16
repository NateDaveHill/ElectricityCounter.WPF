namespace StromzählerContext;

public class Counter
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public List<CounterValue> CounterValues { get; set; }
}