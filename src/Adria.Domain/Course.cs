using System.ComponentModel;

namespace Adria.Domain;

public class Course
{
    public Guid Id { get; }

    private string Name { get; }
    
    private string Category { get; }

    private string Description { get; } //simpele uitleg over de cursus
    
    private List<string> LearnList { get; } //TODO Dit is voor nu een list maar zal later omgezet moeten worden naar een string met comma's die ze splitsen.  
    
    public DateTime Duration { get; private set; }
    
    public CourseLevel Level { get; }
    
    public Decimal Progress { get; }
    

    public Course(Guid id, string name, string category, string description, List<string> learnList, DateTime duration, CourseLevel level)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        LearnList = learnList;
        Duration = duration;
        Level = level;
        Progress = 0;
    }
}