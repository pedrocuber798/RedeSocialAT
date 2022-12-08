namespace RedeSocialAT.Models {
    public class Post {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public DateTime data { get; set; }

        public Perfil Perfil { get; set; }
    }
}
