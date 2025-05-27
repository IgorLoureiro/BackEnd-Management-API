using ManagementAPI.DTO;

namespace ManagementAPI.Helpers
{
    public static class ErrorResponseMappingHelper
    {
        /* Cria erros unitários */
        public static ValidationStyleErrorDto Create(int status, string field, string message)
        {
            return new ValidationStyleErrorDto
            {
                Status = status,
                Errors = new Dictionary<string, List<string>>
                {
                    { field, new List<string> { message } }
                }
            };
        }

        /* Cria erros escalonados */
        public static ValidationStyleErrorDto Create(int status, Dictionary<string, List<string>> errors)
        {
            return new ValidationStyleErrorDto
            {
                Status = status,
                Errors = errors
            };
        }
    }
}
