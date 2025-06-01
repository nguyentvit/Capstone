using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Capstone.Application.Dtos
{
    public class QuestionBaseDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Content { get; init; } = default!;
        public int Difficulty { get; init; }
        public string Type { get; init; } = default!;
        [JsonIgnore]
        public Guid UserId { get; init; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SingleChoiceQuestionDto? SingleChoiceQuestionDto { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TrueFalseQuestionDto? TrueFalseQuestionDto { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MultiChoiceQuestionDto? MultiChoiceQuestionDto { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MatchingQuestionDto? MatchingQuestionDto { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EssayQuestionDto? EssayQuestionDto { get; set; }
    }

    // True False Question
    public class TrueFalseQuestionDto
    {
        public bool IsTrueAnswer { get; init; }
    }

    // Single Choice Question
    public class SingleChoiceQuestionDto
    {
        public List<SingleChoiceQuestionChoiceDto> Choices { get; init; } = new();
        public Guid CorrectAnswerId { get; init; }
    }
    public record SingleChoiceQuestionChoiceDto(Guid Id, string Content);

    // Multi Choice Question
    public class MultiChoiceQuestionDto
    {
        public List<MultiChoiceQuestionChoiceDto> Choices { get; init; } = new();
    }
    public record MultiChoiceQuestionChoiceDto(Guid Id, string Content, bool IsCorrect);

    // Matching Question
    public class MatchingQuestionDto
    {
        public List<MatchingQuestionDtoMatchingPair> MatchingPairs { get; init; } = new();
    }
    public record MatchingQuestionDtoMatchingPair(string Left, string Right, Guid LeftId, Guid RightId);

    public class EssayQuestionDto
    {

    }
}
