using caiobadev_api_arqtool.ViewModels;
using FluentValidation;

namespace caiobadev_apiconcretapp1.ViewModelsValidator {
    public class UsuarioCadastroValidator : AbstractValidator<UsuarioCadastro>
    {
        public UsuarioCadastroValidator()
        {
            RuleFor(x => x.Nome)
                   .NotEmpty().WithMessage("O nome é obigatório.")
                   .NotNull().WithMessage("O nome é obigatório.")
                   .Length(3, 50).WithMessage("O nome deve conter entre 3 e 50 caracteres.");

            RuleFor(x => x.Sobrenome)
                   .NotEmpty().WithMessage("O sobrenome é obigatório.")
                   .NotNull().WithMessage("O sobrenome é obigatório.")
                   .Length(3, 50).WithMessage("O sobrenome deve conter entre 3 e 50 caracteres.");

            RuleFor(x => x.DataNascimento)
                   .LessThanOrEqualTo(DateTime.Today).WithMessage("A data de nascimento não pode ser maior que a data atual.")
                   .Must(ValidaMaiorIdade).WithMessage("A data de nascimento deve corresponder a uma idade de pelo menos 18 anos.");

            RuleFor(x => x.Senha)
                   .NotEmpty().WithMessage("A senha é obigatória.")
                   .Length(5, 20).WithMessage("A senha deve conter entre 5 e 20 caracteres.")
                   .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.ConfirmacaoSenha)
                   .NotEmpty().WithMessage("A confirmação de senha é obigatória.")
                   .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.")
                   .Equal(x => x.Senha).WithMessage("A senha e a confirmação devem ser iguais.");
        }


        public static bool ValidarTelefone(string telefone)
        {
            // Verifica se o telefone tem 11 dígitos
            //if (telefone.Length != 11)
            //{
            //    return false;
            //}

            // Verifica se os dígitos são válidos
            for (int i = 0; i < 10; i++)
            {
                if (telefone[i] < '0' || telefone[i] > '9')
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidaMaiorIdade(DateTime dataNascimento)
        {
            DateTime dataAtual = DateTime.Today;
            int idade = dataAtual.Year - dataNascimento.Year;

            // Verificar se o aniversário já ocorreu este ano
            if (dataAtual.Month < dataNascimento.Month || (dataAtual.Month == dataNascimento.Month && dataAtual.Day < dataNascimento.Day))
            {
                idade--;
            }

            if (idade >= 18)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}
