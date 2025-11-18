namespace Adria.Domain;

public class Module
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public int Position { get; set; }
    public string Content { get; set; }

    public Module(Guid moduleId, Guid courseId, int position, string content)
    {
        Id = moduleId;
        CourseId = courseId;
        Position = position;
        Content = content;
    }

    public Module(Guid courseId, int position, string content)
    :this(Guid.NewGuid(), courseId, position, content)
    {
    }
}