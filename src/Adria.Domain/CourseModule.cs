namespace Adria.Domain;

public class CourseModule
{
    public Guid Id { get; }
    
    public string Name { get; }

    public CourseModule(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public CourseModule(string name) :  this(Guid.NewGuid(), name)
    {
       
    }
}