namespace Capstone.Application.ExamDomain.Queries.GetExamVersionById
{
    public record GetExamVersionByIdQuery(Guid UserId, Guid ExamVersionId) : IQuery<GetExamVersionByIdResult>;
    public record GetExamVersionByIdResult(int Duration, string Code, string SubjectName, List<GetExamVersionByIdQuestion> Questions);
    public record GetExamVersionByIdQuestion(QuestionBaseDto Data, int Order, double PointPerCorrect, double PointPerInCorrect);
}
