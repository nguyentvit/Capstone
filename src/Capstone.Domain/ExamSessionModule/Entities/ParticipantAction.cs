using Capstone.Domain.ExamSessionModule.Enums;
using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Domain.ExamSessionModule.Entities
{
    public class ParticipantAction : Entity<ParticipantActionId>
    {
        public ActionType ActionType { get; private set; } = default!;
        private ParticipantAction() { }
        private ParticipantAction(ActionType actionType)
        {
            Id = ParticipantActionId.Of(Guid.NewGuid());
            ActionType = actionType;
        }
        public static ParticipantAction Of(ActionType actionType)
        {
            return new ParticipantAction(actionType);
        }
    }
}
