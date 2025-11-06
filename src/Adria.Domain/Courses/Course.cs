using System.ComponentModel;

namespace Adria.Domain;

public class Course
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    //simpele uitleg over de cursus
    public List<string> LearnList { get; private set; }
    //Dit is voor nu een list maar zal later omgezet moeten worden naar een string met comma's die ze splitsen.  
    public int Duration { get; private set; }
    //Duratie van een cursus in uur
    public CourseLevel Level { get; private set; }
    public List<CourseModule> Modules { get; private set; }
    public int PointsOnCompletion { get; private set; }


    public Course(Guid id, string name, string category, string description, List<string> learnList, int duration,
        int level, List<CourseModule> modules, int pointsOnCompletion)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        LearnList = learnList;
        Duration = duration;
        Level = (CourseLevel)level;
        Modules = modules;
        PointsOnCompletion = pointsOnCompletion;
    }

    public Course(string name, string category, string description, List<string> learnList, int duration,
        int level, List<CourseModule> modules, int pointsOnCompletion)
        : this(Guid.NewGuid(), name, category, description, learnList, duration, level, modules, pointsOnCompletion)
    {
    }
}