using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Process.DTOs
{
    public class StatusResponseDTO
    {
        public bool Success => string.IsNullOrEmpty(Error);
        public object? Data { get; set; }
        public string? Error { get; set; }
        public static StatusResponseDTO GetError(string error) => new StatusResponseDTO {Error = error };
        public static StatusResponseDTO Ok(object? data) => new StatusResponseDTO { Error = string.Empty, Data = data };
        public static StatusResponseDTO NotFoundError() => new StatusResponseDTO { Error = "Not Found" };
      
    }
}
