using Adria.Domain;

namespace UnitTests.Mocks
{
    public class MockUserQARepository : IUserQARepository
    {
        public Guid LastSavedAnswerId { get; private set; }
        private readonly Dictionary<(Guid questionId, Guid answerId), Guid> _qaIds = new();
        public int GetProgressValue { get; set; } = 42;

        public Task<Guid> GetQAID(Guid questionId, Guid answerId)
        {
            var key = (questionId, answerId);
            if (!_qaIds.TryGetValue(key, out var qaid))
            {
                qaid = Guid.NewGuid();
                _qaIds[key] = qaid;
            }

            return Task.FromResult(qaid);
        }

        public Task<Guid> GetCorrectAnswerId(Guid questionId)
        {
            foreach (var key in _qaIds.Keys)
            {
                if (key.questionId == questionId)
                    return Task.FromResult(key.answerId);
            }

            return Task.FromResult(Guid.Empty);
        }

        public Task<Guid> Save(Guid userId, Guid answerId, Guid questionId)
        {
            LastSavedAnswerId = answerId;
            _ = GetQAID(questionId, answerId);
            return Task.FromResult(answerId);
        }

        public Task<int> GetProgress(Guid userId, Guid courseId)
        {
            return Task.FromResult(GetProgressValue);
        }
    }
}