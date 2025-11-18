using System.ComponentModel;

namespace Adria.Domain;

public class Course
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public CourseLevel Level { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public IList<string> Tags { get; private set; }
    public int PointsOnCompletion { get; private set; }
    public IList<string> LearnList { get; private set; } // Summary of things you'll learn
    public int Duration { get; private set; } // Duration in hours
    public Guid CoverImageId { get; private set; }
    public Guid BannerImageId { get; private set; }
    public Boolean SubscriptionRequired { get; private set; }

    public Course(Guid id, string name, CourseLevel level, string description, string category, IList<string> tags, int pointsOnCompletion, IList<string> learnList, int duration, Guid coverImageId, Guid bannerImageId, bool subscriptionRequired)
    {
        Id = id;
        Name = name;
        Level = level;
        Description = description;
        Category = category;
        Tags = tags;
        PointsOnCompletion = pointsOnCompletion;
        LearnList = learnList;
        Duration = duration;
        CoverImageId = coverImageId;
        BannerImageId = bannerImageId;
        SubscriptionRequired = subscriptionRequired;
    }


    public Course(string name, CourseLevel level, string description, string category, IList<string> tags, int pointsOnCompletion, IList<string> learnList, int duration, Guid coverImageId, Guid bannerImageId, Boolean subscriptionRequired)
        : this(Guid.NewGuid(), name, level, description, category, tags, pointsOnCompletion, learnList, duration, coverImageId, bannerImageId, subscriptionRequired)
    {
    }
}