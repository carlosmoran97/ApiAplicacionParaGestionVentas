using System;
using System.Collections.Generic;

namespace backend
{
    public partial class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal? PrecioPromocional { get; set; }
        public string Codigo { get; set; }
        public decimal? Costo { get; set; }
        public string Unidad { get; set; }
        public bool? VerCatalogo { get; set; }
        public int? IdCategoria { get; set; }
        public string Imagen { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
    }
}
