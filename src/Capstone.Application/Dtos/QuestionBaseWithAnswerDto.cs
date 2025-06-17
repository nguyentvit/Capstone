using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Capstone.Application.Dtos
{
    public class QuestionBaseWithAnswerDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = default!;
        public string Content { get; init; } = default!;
        public int Difficulty { get; init; }
        public string Type { get; init; } = default!;
        public double Score { get; init; } = default!;
        [JsonIgnore]
        public Guid UserId { get; init; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SingleChoiceQuestionWithAnswerDto? SingleChoiceQuestionDto { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TrueFalseQuestionWithAnswerDto? TrueFalseQuestionDto { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MultiChoiceQuestionWithAnswerDto? MultiChoiceQuestionDto { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MatchingQuestionWithAnswerDto? MatchingQuestionDto { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public EssayQuestionWithAnswerDto? EssayQuestionDto { get; set; }
    }

    // True False Question
    public class TrueFalseQuestionWithAnswerDto
    {
        public bool IsTrueAnswer { get; init; }
        public TrueFalseAnswer? Answer { get; init; }
    }

    // Single Choice Question
    public class SingleChoiceQuestionWithAnswerDto
    {
        public List<SingleChoiceQuestionChoiceDto> Choices { get; init; } = new();
        public Guid CorrectAnswerId { get; init; }
        public SingleChoiceAnswer? Answer { get; init; }
    }
    public record SingleChoiceQuestionChoiceWithAnswerDto(Guid Id, string Content);

    // Multi Choice Question
    public class MultiChoiceQuestionWithAnswerDto
    {
        public List<MultiChoiceQuestionChoiceWithAnswerDto> Choices { get; init; } = new();
        public MultiChoiceAnswer? Answer { get; init; }
    }
    public record MultiChoiceQuestionChoiceWithAnswerDto(Guid Id, string Content, bool IsCorrect);

    // Matching Question
    public class MatchingQuestionWithAnswerDto
    {
        public List<MatchingQuestionDtoMatchingPairWithAnswer> MatchingPairs { get; init; } = new();
        public MatchingPairAnswer? Answer { get; init; }
    }
    public record MatchingQuestionDtoMatchingPairWithAnswer(string Left, string Right, Guid LeftId, Guid RightId);

    public class EssayQuestionWithAnswerDto
    {
        public EssayAnswer? Answer { get; init; }
    }
}
