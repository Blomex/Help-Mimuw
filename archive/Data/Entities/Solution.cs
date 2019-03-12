namespace archive.Data.Entities
{
    public class Solution
    {
        public int Id { get; set; }
        public string Content { get; set; }
        
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}