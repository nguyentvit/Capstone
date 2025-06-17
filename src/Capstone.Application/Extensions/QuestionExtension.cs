using Capstone.Domain.QuestionDomain.Common.Models;
using Capstone.Domain.QuestionDomain.EssayQuestion.Models;
using Capstone.Domain.QuestionDomain.MatchingQuestion.Models;
using Capstone.Domain.QuestionDomain.MultiChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.SingleChoiceQuestion.Models;
using Capstone.Domain.QuestionDomain.TrueFalseQuestion.Models;

namespace Capstone.Application.Extensions
{
    public static class QuestionExtension
    {
        public static QuestionBaseDto ConvertToQuestionDto(Question question)
        {
            var questionDto = new QuestionBaseDto
            {
                Id = question.Id.Value,
                Title = question.Title.Value,
                Content = question.Content.Value,
                Difficulty = (int)question.Difficulty,
                Type = question.Type.ToString(),
                UserId = question.UserId.Value,
            };

            switch (question)
            {
                case TrueFalseQuestion tf:
                    questionDto.TrueFalseQuestionDto = new TrueFalseQuestionDto
                    {
                        IsTrueAnswer = tf.IsTrueAnswer.Value
                    };
                    break;
                case SingleChoiceQuestion sc:
                    questionDto.SingleChoiceQuestionDto = new SingleChoiceQuestionDto
                    {
                        Choices = sc.Choices.Select(c => new SingleChoiceQuestionChoiceDto(c.Id.Value, c.Content.Value)).ToList(),
                        CorrectAnswerId = sc.CorrectAnswerId.Value,
                    };
                    break;
                case MultiChoiceQuestion mc:
                    questionDto.MultiChoiceQuestionDto = new MultiChoiceQuestionDto
                    {
                        Choices = mc.Choices.Select(c => new MultiChoiceQuestionChoiceDto(c.Id.Value, c.Content.Value, c.IsCorrect.Value)).ToList()
                    };
                    break;
                case MatchingQuestion mq:
                    questionDto.MatchingQuestionDto = new MatchingQuestionDto
                    {
                        MatchingPairs = mq.MatchingPairs.Select(p => new MatchingQuestionDtoMatchingPair(p.Left.Value, p.Right.Value, p.Left.Id, p.Right.Id)).ToList()
                    };
                    break;
                case EssayQuestion eq:
                    break;
            }
            return questionDto;
        }
        public static double CalculateCorrectnessPercentage(Question question, string rawAnswer)
        {
            switch (question)
            {
                case TrueFalseQuestion tf:
                    var answerTrueFalse = JsonConvert.DeserializeObject<TrueFalseAnswer>(rawAnswer);
                    if (answerTrueFalse == null)
                        throw new BadRequestException("Không thể chuyển đổi");

                    return answerTrueFalse.Answer == tf.IsTrueAnswer.Value ? 1.0 : 0.0;

                case SingleChoiceQuestion sc:
                    var answerSingleChoice = JsonConvert.DeserializeObject<SingleChoiceAnswer>(rawAnswer);
                    if (answerSingleChoice == null)
                        throw new BadRequestException("Không thể chuyển đổi");

                    return sc.CorrectAnswerId.Value == answerSingleChoice.Answer ? 1.0 : 0.0;

                case MultiChoiceQuestion mc:
                    var answerMultiChoice = JsonConvert.DeserializeObject<MultiChoiceAnswer>(rawAnswer);
                    if (answerMultiChoice == null)
                        throw new BadRequestException("Không thể chuyển đổi");

                    var correctIds = mc.Choices
                        .Where(c => c.IsCorrect.Value)
                        .Select(c => c.Id.Value)
                        .ToHashSet();

                    var userAnswer = answerMultiChoice.Answer.ToHashSet();

                    if (!userAnswer.Any())
                        return 0.0;

                    var correctSelected = userAnswer.Count(id => correctIds.Contains(id));
                    var incorrectSelected = userAnswer.Count(id => !correctIds.Contains(id));

                    var totalCorrect = correctIds.Count;

                    double score = (double)correctSelected / totalCorrect;

                    if (incorrectSelected > 0)
                        score -= ((double)incorrectSelected / userAnswer.Count);

                    return Math.Max(0.0, Math.Round(score, 2));

                case MatchingQuestion mq:
                    var answerMatching = JsonConvert.DeserializeObject<MatchingPairAnswer>(rawAnswer);
                    if (answerMatching == null)
                        throw new BadRequestException("Không thể chuyển đổi");

                    var correctPairs = mq.MatchingPairs.ToDictionary(p => p.Left.Id, p => p.Right.Id);
                    var matchingAnswer = answerMatching.Answer;

                    if (matchingAnswer.Count == 0)
                        return 0.0;

                    int totalPairs = correctPairs.Count;
                    int correctCount = matchingAnswer.Count(pair =>
                        correctPairs.TryGetValue(pair.Key, out var correctRightId) &&
                        correctRightId == pair.Value);

                    return Math.Round((double)correctCount / totalPairs, 2);

                case EssayQuestion eq:
                    return 0;
            }

            return 0.0;
        }
        public static QuestionBaseWithAnswerDto ConvertToQuestionWithAnswerDto(Question question, string rawAnswer, double score)
        {
            var questionDto = new QuestionBaseWithAnswerDto
            {
                Id = question.Id.Value,
                Title = question.Title.Value,
                Content = question.Content.Value,
                Difficulty = (int)question.Difficulty,
                Type = question.Type.ToString(),
                UserId = question.UserId.Value,
                Score = score
            };

            switch (question)
            {
                case TrueFalseQuestion tf:
                    var answerTrueFalse = JsonConvert.DeserializeObject<TrueFalseAnswer>(rawAnswer);
                    questionDto.TrueFalseQuestionDto = new TrueFalseQuestionWithAnswerDto
                    {
                        IsTrueAnswer = tf.IsTrueAnswer.Value,
                        Answer = answerTrueFalse
                    };
                    break;
                case SingleChoiceQuestion sc:
                    var answerSingleChoice = JsonConvert.DeserializeObject<SingleChoiceAnswer>(rawAnswer);
                    questionDto.SingleChoiceQuestionDto = new SingleChoiceQuestionWithAnswerDto
                    {
                        Choices = sc.Choices.Select(c => new SingleChoiceQuestionChoiceDto(c.Id.Value, c.Content.Value)).ToList(),
                        CorrectAnswerId = sc.CorrectAnswerId.Value,
                        Answer = answerSingleChoice
                    };
                    break;
                case MultiChoiceQuestion mc:
                    var answerMulti = JsonConvert.DeserializeObject<MultiChoiceAnswer>(rawAnswer);
                    questionDto.MultiChoiceQuestionDto = new MultiChoiceQuestionWithAnswerDto
                    {
                        Choices = mc.Choices.Select(c => new MultiChoiceQuestionChoiceWithAnswerDto(c.Id.Value, c.Content.Value, c.IsCorrect.Value)).ToList(),
                        Answer = answerMulti
                    };
                    break;
                case MatchingQuestion mq:
                    var answerMatching = JsonConvert.DeserializeObject<MatchingPairAnswer>(rawAnswer);
                    questionDto.MatchingQuestionDto = new MatchingQuestionWithAnswerDto
                    {
                        MatchingPairs = mq.MatchingPairs.Select(p => new MatchingQuestionDtoMatchingPairWithAnswer(p.Left.Value, p.Right.Value, p.Left.Id, p.Right.Id)).ToList(),
                        Answer = answerMatching
                    };
                    break;
                case EssayQuestion eq:
                    var essayAnswer = JsonConvert.DeserializeObject<EssayAnswer>(rawAnswer);
                    questionDto.EssayQuestionDto = new EssayQuestionWithAnswerDto
                    {
                        Answer = essayAnswer
                    };
                    break;
            }
            return questionDto;
        }
    }
}
