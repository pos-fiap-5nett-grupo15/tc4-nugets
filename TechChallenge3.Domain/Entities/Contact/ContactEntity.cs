using TechChallenge3.Domain.Enums;

namespace TechChallenge3.Domain.Entities.Contact
{
    public class ContactEntity : BaseEntity
    {
        public ContactEntity() { }

        public ContactEntity(
            string nome,
            string email,
            int ddd,
            int telefone,
            ContactSituationEnum? situacaoAtual,
            ContactSituationEnum? situacaoAnterior)
            : this()
        {
            Nome = nome;
            Email = email;
            Ddd = ddd;
            Telefone = telefone;
            SituacaoAtual = situacaoAtual;
            SituacaoAnterior = situacaoAnterior;
        }

        public string Nome { get; init; }
        public string Email { get; init; }
        public int Ddd { get; init; }
        public int Telefone { get; init; }
        public ContactSituationEnum? SituacaoAnterior { get; init; }
        public ContactSituationEnum? SituacaoAtual { get; init; }
    }
}
