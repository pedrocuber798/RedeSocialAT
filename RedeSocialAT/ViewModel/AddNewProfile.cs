using RedeSocialAT.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace RedeSocialAT.ViewModel {
    public class AddNewProfile {
        public List<Perfil> Perfil { get; set; }


        [Required(ErrorMessage = "Campo Nome Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório a seleção da marca do Carro")]
        public IFormFile Fotourl { get; set; }

    }
}
