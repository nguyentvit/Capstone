using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Domain.ExamTemplateModule.ValueObjects
{
    public record DifficultySectionConfig
    {
        public QuestionDifficulty Difficulty { get; private set; }
        private readonly List<QuestionTypeConfig> _questionTypeConfigs = new();
        public IReadOnlyList<QuestionTypeConfig> QuestionTypeConfigs => _questionTypeConfigs.AsReadOnly();
        private DifficultySectionConfig() { }
        private DifficultySectionConfig(QuestionDifficulty difficulty, List<QuestionTypeConfig> questionTypeConfigs)
        {
            Difficulty = difficulty;
            _questionTypeConfigs = questionTypeConfigs;
        }
        public static DifficultySectionConfig Of(QuestionDifficulty difficulty, List<QuestionTypeConfig> questionTypeConfigs)
        {
            return new DifficultySectionConfig(difficulty, questionTypeConfigs);
        }
        public void UpdateQuestionTypeConfig(QuestionTypeConfig updated)
        {
            var index = _questionTypeConfigs.FindIndex(x => x.Type == updated.Type);
            if (index >= 0)
                _questionTypeConfigs[index] = updated;
            else
                _questionTypeConfigs.Add(updated);
        }
    }
}
