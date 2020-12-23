using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.DTOs
{
    public class EventoDto
    {
         public int Id { get; set; }

        [Required (ErrorMessage = "Obrigatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "100 e 3 caracteres")]
        public string Local { get; set; }
        
        public DateTime DataEvento { get; set; }
        
        [Required (ErrorMessage = "Obrigatorio")]
        public string Tema { get; set; }
        
        [Range(2,12000, ErrorMessage = "Quantidade de pessoas é entre 2 e 120000")]
        public int QtdPessoas { get; set; }
        
        public string ImageURL { get; set; }
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}