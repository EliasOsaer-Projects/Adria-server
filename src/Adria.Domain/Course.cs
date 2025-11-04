using System.ComponentModel;

namespace Adria.Domain;

public class Course
{
    public Guid Id { get; }
    private string Name { get; }
    private string Category { get; }
    private string Description { get; } 
    //simpele uitleg over de cursus
    private List<string> LearnList { get; } 
    //Dit is voor nu een list maar zal later omgezet moeten worden naar een string met comma's die ze splitsen.  
    private int Duration { get; } 
    //Duratie van een cursus in uur
    private CourseLevel Level { get; }
    private List<CourseModule> Modules { get; }
    private Guid ImageId { get; }

    public Course(Guid id, string name, string category, string description, List<string> learnList, int duration,
        CourseLevel level, List<CourseModule> modules,  Guid imageId)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        LearnList = learnList;
        Duration = duration;
        Level = level;
        Modules = modules;
        ImageId = imageId;
    }
}