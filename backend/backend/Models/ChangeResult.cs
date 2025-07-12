namespace backend.Models
{
    public class ChangeResult
    {

        public string Message { get; set; }
        public bool Success { get; set; }
        public Dictionary<int, int> ChangeBreakdown { get; set; } = new();
    }
}