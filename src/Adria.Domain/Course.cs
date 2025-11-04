using System.ComponentModel;

namespace Adria.Domain;

public class Course
{
    public Guid Id { get; }

    private string Name { get; }
    
    private string Category { get; }

    private string Description { get; } //simpele uitleg over de cursus
    
    private List<string> LearnList { get; } //Dit is voor nu een list maar zal later omgezet moeten worden naar een string met comma's die ze splitsen.  
    
    private DateTime Duration { get; }
    
    private CourseLevel Level { get; }
    
    private Decimal Progress { get; }
    
    private List<CourseModule> Modules { get; }
    

    public Course(Guid id, string name, string category, string description, List<string> learnList, DateTime duration, CourseLevel level, List<CourseModule> modules)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        LearnList = learnList;
        Duration = duration;
        Level = level;
        Progress = 0;
        Modules = modules;
    }
}