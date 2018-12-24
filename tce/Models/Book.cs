using System;

namespace tce.Models
{
    public class Book
    {
  
        public long Id { get; set; }
        public string ISBN { get; set; }        // Codigo unico do livro (International Standard Book Number) - UK
        public string Name { get; set; }        // Nome do livro
        public Double Price { get; set; }      // Preco do livro
        public DateTime Published { get; set; } // Data de publicacao
        public string Avatar { get; set; }      // URL do arquivo de avatar (Imagem da Capa)
        // (ISBN, Autor, Nome, Preço, Data Publicação, Imagem da Capa);

    }
}
