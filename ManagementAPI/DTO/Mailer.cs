using System.ComponentModel.DataAnnotations;

namespace ManagementAPI.DTO;

public class MailerRequestDto
{

    [Required(ErrorMessage = "O campo 'Sender' é obrigatório.")]
    [MinLength(1, ErrorMessage = "O campo 'Sender' deve conter pelo menos 1 caractere")]
    public string? Sender { get; set; }

    [Required(ErrorMessage = "O campo 'To' é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo 'To' deve ser um email válido")]
    [MinLength(1, ErrorMessage = "O campo 'To' deve conter pelo menos 1 caractere")]
    public string? To { get; set; }

    [Required(ErrorMessage = "O campo 'Subject' é obrigatório.")]
    [MinLength(1, ErrorMessage = "O campo 'Subject' não pode estar vazio.")]
    public string? Subject {get; set; }

    [Required(ErrorMessage = "O campo 'Body' é obrigatório.")]
    [MinLength(1, ErrorMessage = "O campo 'Body' não pode estar vazio.")]
    public string? Body {get; set; }

}