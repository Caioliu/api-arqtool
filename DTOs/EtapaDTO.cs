using caiobadev_api_arqtool.Models;

namespace caiobadev_api_arqtool.DTOs
{
    public class EtapaDTO
    {
        public int EtapaId { get; set; }
        public int ProjetoId { get; set; }
        public ICollection<Atividade> Atividades { get; set; }
    }
}
