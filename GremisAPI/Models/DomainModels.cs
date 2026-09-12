using System;
using System.Collections.Generic;

namespace GremisAPI.Models
{
    // ============================================
    // ROLE & PERMISSION MODELS
    // ============================================

    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RolePermission> RolePermissions { get; set; }
    }

    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }

    // ============================================
    // USER MODELS
    // ============================================

    public class User
    {
        public long Id { get; set; }
        public string Uuid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; } = "active"; // active, inactive, suspended
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Role Role { get; set; }
        public MigrantWorker MigrantWorker { get; set; }
        public FamilyMember FamilyMember { get; set; }
        public Professional Professional { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ForumPost> ForumPosts { get; set; }
        public ICollection<ForumComment> ForumComments { get; set; }
        public ICollection<CounselingSession> CounselingSessions { get; set; }
        public ICollection<ActivityLog> ActivityLogs { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }

    // ============================================
    // MIGRANT WORKER & FAMILY MODELS
    // ============================================

    public class MigrantWorker
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IdNumber { get; set; }
        public string PassportNumber { get; set; }
        public string CountryDestination { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string Occupation { get; set; }
        public decimal MonthlyIncome { get; set; }
        public string Status { get; set; } = "working"; // preparation, working, return, retired
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public ICollection<FamilyMember> FamilyMembers { get; set; }
        public ICollection<Remittance> Remittances { get; set; }
        public ICollection<BusinessPlan> BusinessPlans { get; set; }
        public ICollection<FamilyBudget> FamilyBudgets { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; }
        public ICollection<MigrantWorkerVillage> MigrantWorkerVillages { get; set; }
    }

    public class FamilyMember
    {
        public long Id { get; set; }
        public long MigrantWorkerId { get; set; }
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public string Relationship { get; set; } // spouse, child, parent, sibling, other
        public DateTime? DateOfBirth { get; set; }
        public string EducationLevel { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public MigrantWorker MigrantWorker { get; set; }
        public User User { get; set; }
        public ICollection<ScreeningResponse> ScreeningResponses { get; set; }
        public ICollection<CounselingSession> CounselingSessions { get; set; }
        public ICollection<ChildDevelopmentRecord> ChildDevelopmentRecords { get; set; }
    }

    public class Village
    {
        public int Id { get; set; }
        public string VillageName { get; set; }
        public string District { get; set; }
        public string Regency { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MigrantWorkerVillage> MigrantWorkerVillages { get; set; }
        public ICollection<ProfessionalVillage> ProfessionalVillages { get; set; }
        public ICollection<CollaborativeProgram> CollaborativePrograms { get; set; }
    }

    public class MigrantWorkerVillage
    {
        public long MigrantWorkerId { get; set; }
        public int VillageId { get; set; }

        public MigrantWorker MigrantWorker { get; set; }
        public Village Village { get; set; }
    }

    // ============================================
    // EKONOMI KELUARGA MODELS
    // ============================================

    public class Remittance
    {
        public long Id { get; set; }
        public long MigrantWorkerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "IDR";
        public DateTime RemittanceDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string TransferMethod { get; set; } // bank, money_transfer, cash, digital_wallet
        public string Description { get; set; }
        public string Status { get; set; } = "pending"; // pending, completed, failed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public MigrantWorker MigrantWorker { get; set; }
    }

    public class FamilyBudget
    {
        public long Id { get; set; }
        public long MigrantWorkerId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? TotalExpense { get; set; }
        public decimal? Savings { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public MigrantWorker MigrantWorker { get; set; }
        public ICollection<BudgetDetail> BudgetDetails { get; set; }
    }

    public class BudgetCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; } // income, expense
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BudgetDetail> BudgetDetails { get; set; }
    }

    public class BudgetDetail
    {
        public long Id { get; set; }
        public long FamilyBudgetId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public FamilyBudget FamilyBudget { get; set; }
        public BudgetCategory Category { get; set; }
    }

    public class BusinessPlan
    {
        public long Id { get; set; }
        public long MigrantWorkerId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public string Description { get; set; }
        public decimal? StartCapital { get; set; }
        public decimal? ProjectedRevenue { get; set; }
        public DateTime? TargetLaunchDate { get; set; }
        public string Status { get; set; } = "planning"; // planning, active, paused, closed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public MigrantWorker MigrantWorker { get; set; }
        public ICollection<BusinessProgress> BusinessProgressRecords { get; set; }
    }

    public class BusinessProgress
    {
        public long Id { get; set; }
        public long BusinessPlanId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal? ActualRevenue { get; set; }
        public decimal? ActualExpense { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public BusinessPlan BusinessPlan { get; set; }
    }

    // ============================================
    // PSIKOLOGI REMAJA MODELS
    // ============================================

    public class ScreeningForm
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string FormType { get; set; } // emotional, social, adaptation, stress, general
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ScreeningQuestion> ScreeningQuestions { get; set; }
        public ICollection<ScreeningResponse> ScreeningResponses { get; set; }
    }

    public class ScreeningQuestion
    {
        public int Id { get; set; }
        public int ScreeningFormId { get; set; }
        public string QuestionText { get; set; }
        public int? QuestionOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ScreeningForm ScreeningForm { get; set; }
        public ICollection<ScreeningAnswer> ScreeningAnswers { get; set; }
    }

    public class ScreeningResponse
    {
        public long Id { get; set; }
        public long FamilyMemberId { get; set; }
        public int ScreeningFormId { get; set; }
        public DateTime ResponseDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public FamilyMember FamilyMember { get; set; }
        public ScreeningForm ScreeningForm { get; set; }
        public ScreeningResult ScreeningResult { get; set; }
        public ICollection<ScreeningAnswer> ScreeningAnswers { get; set; }
    }

    public class ScreeningAnswer
    {
        public long Id { get; set; }
        public long ScreeningResponseId { get; set; }
        public int ScreeningQuestionId { get; set; }
        public string AnswerValue { get; set; }
        public int? AnswerScore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ScreeningResponse ScreeningResponse { get; set; }
        public ScreeningQuestion ScreeningQuestion { get; set; }
    }

    public class ScreeningResult
    {
        public long Id { get; set; }
        public long ScreeningResponseId { get; set; }
        public int TotalScore { get; set; }
        public string ResultCategory { get; set; } // excellent, good, fair, needs_intervention, critical
        public string Interpretation { get; set; }
        public string Recommendation { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ScreeningResponse ScreeningResponse { get; set; }
    }

    public class CounselingSession
    {
        public long Id { get; set; }
        public long FamilyMemberId { get; set; }
        public long PsychologistId { get; set; }
        public DateTime SessionDate { get; set; }
        public string SessionType { get; set; } // online, offline
        public int? DurationMinutes { get; set; }
        public string Status { get; set; } = "scheduled"; // scheduled, completed, cancelled
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public FamilyMember FamilyMember { get; set; }
        public User Psychologist { get; set; }
        public CounselingRecord CounselingRecord { get; set; }
    }

    public class CounselingRecord
    {
        public long Id { get; set; }
        public long CounselingSessionId { get; set; }
        public string SessionSummary { get; set; }
        public string IssuesDiscussed { get; set; }
        public string InterventionPlan { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CounselingSession CounselingSession { get; set; }
    }

    // ============================================
    // KOMUNIKASI KELUARGA MODELS
    // ============================================

    public class ChatRoom
    {
        public long Id { get; set; }
        public long MigrantWorkerId { get; set; }
        public string RoomName { get; set; }
        public string RoomType { get; set; } // one_to_one, group_family
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public MigrantWorker MigrantWorker { get; set; }
        public ICollection<ChatRoomMember> ChatRoomMembers { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

    public class ChatRoomMember
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public long UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public ChatRoom ChatRoom { get; set; }
        public User User { get; set; }
    }

    public class Message
    {
        public long Id { get; set; }
        public long ChatRoomId { get; set; }
        public long SenderId { get; set; }
        public string MessageText { get; set; }
        public string MediaUrl { get; set; }
        public string MessageType { get; set; } = "text"; // text, image, video, file
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ChatRoom ChatRoom { get; set; }
        public User Sender { get; set; }
    }

    public class ChildDevelopmentRecord
    {
        public long Id { get; set; }
        public long FamilyMemberId { get; set; }
        public DateTime RecordDate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string AcademicPerformance { get; set; }
        public string BehavioralNotes { get; set; }
        public string HealthStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public FamilyMember FamilyMember { get; set; }
    }

    // ============================================
    // KOLABORASI KOMUNITAS MODELS
    // ============================================

    public class Professional
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ProfessionType { get; set; } // teacher_counselor, village_admin, volunteer, psychologist
        public string Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public string CertificationNumber { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public ICollection<ProfessionalVillage> ProfessionalVillages { get; set; }
        public ICollection<CollaborativeProgram> CollaborativePrograms { get; set; }
    }

    public class ProfessionalVillage
    {
        public long ProfessionalId { get; set; }
        public int VillageId { get; set; }

        public Professional Professional { get; set; }
        public Village Village { get; set; }
    }

    public class ForumPost
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string ForumCategory { get; set; } // general, economic_tips, psychology, education, health
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; } = "published"; // published, draft, archived
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User Author { get; set; }
        public ICollection<ForumComment> ForumComments { get; set; }
    }

    public class ForumComment
    {
        public long Id { get; set; }
        public long ForumPostId { get; set; }
        public long AuthorId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ForumPost ForumPost { get; set; }
        public User Author { get; set; }
    }

    public class CollaborativeProgram
    {
        public long Id { get; set; }
        public string ProgramName { get; set; }
        public string ProgramDescription { get; set; }
        public int VillageId { get; set; }
        public long CoordinatorId { get; set; }
        public string ProgramType { get; set; } // training, counseling, support_group, workshop
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "planning"; // planning, active, completed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Village Village { get; set; }
        public Professional Coordinator { get; set; }
        public ICollection<ProgramParticipant> ProgramParticipants { get; set; }
    }

    public class ProgramParticipant
    {
        public long Id { get; set; }
        public long CollaborativeProgramId { get; set; }
        public long UserId { get; set; }
        public string ParticipationStatus { get; set; } = "registered"; // registered, active, completed, dropped
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public CollaborativeProgram CollaborativeProgram { get; set; }
        public User User { get; set; }
    }

    // ============================================
    // DASHBOARD & REPORTING MODELS
    // ============================================

    public class UserDashboard
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal TotalRemittanceReceived { get; set; }
        public int FamilyMemberCount { get; set; }
        public int? LatestPsychologicalScore { get; set; }
        public string BusinessStatus { get; set; }
        public int ActiveCounselingSessions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }

    public class ActivityLog
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }

    public class Notification
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string NotificationType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RelatedEntityType { get; set; }
        public long? RelatedEntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        public User User { get; set; }
    }
}
