namespace models
{
    public class Comment
    {
        public string? CommentReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Id { get; set; }
        public string? TaskId { get; set; }
        public string? Text { get; set; }
    }
}