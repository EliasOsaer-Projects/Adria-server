namespace Adria.Domain;

public class CourseModule
{
    public Guid Id { get; }
    
    public string Name { get; }

    public CourseModule(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}