using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.ExamTemplateModule.ValueObjects;

namespace Capstone.Domain.ExamTemplateModule.Entities
{
    public class ExamTemplateSection : Entity<ExamTemplateSectionId>
    {
        private readonly List<DifficultySectionConfig> _difficultyConfigs = new();
        public IReadOnlyList<DifficultySectionConfig> DifficultyConfigs => _difficultyConfigs.AsReadOnly();
        public ChapterId ChapterId { get; private set; } = default!;
        private ExamTemplateSection() { }
        private ExamTemplateSection(ChapterId chapterId, List<DifficultySectionConfig> difficultyConfigs)
        {
            Id = ExamTemplateSectionId.Of(Guid.NewGuid());
            ChapterId = chapterId;
            _difficultyConfigs = difficultyConfigs;
        }
        public static ExamTemplateSection Of(ChapterId chapterId, List<DifficultySectionConfig> difficultyConfigs)
        {
            return new ExamTemplateSection(chapterId, difficultyConfigs);
        }
        public void UpdateDifficultyConfig(DifficultySectionConfig updated)
        {
            var index = _difficultyConfigs.FindIndex(x => x.Difficulty == updated.Difficulty);
            if (index >= 0)
                _difficultyConfigs[index] = updated;
            else
                _difficultyConfigs.Add(updated);
        }
    }
}
