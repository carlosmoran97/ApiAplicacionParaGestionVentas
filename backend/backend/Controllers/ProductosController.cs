using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend;
using System.IO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly GestionVentasBDContext _context;

        public ProductosController(GestionVentasBDContext context)
        {
            _context = context;
        }

        // GET: api/Producto
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Producto>>> GetProducto()
        //{
        //    return await _context.Producto.ToListAsync();
        //}

        
        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // PUT: api/Producto/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Producto
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            
            //Agregando imagen a carpeta
            string filtePath = Path.GetFullPath(@"Images");
            //string nombreImagen = producto.Nombre.Replace(" ", "");
            Guid nombreImagen = Guid.NewGuid();
            string rutaImagen = filtePath + "\\" +  nombreImagen + ".png";
            string imagenBase = producto.Imagen.Remove(0,22);
            byte[] archivoBase64 = Convert.FromBase64String(imagenBase);
            System.IO.File.WriteAllBytes(rutaImagen, archivoBase64);

            producto.Imagen = "/Images/" + nombreImagen + ".png";
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
            

            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        // DELETE: api/Producto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return producto;
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        // GET: api/Productos/Paginado/1&10
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductoPaginado([FromQuery(Name = "page")] int page, [FromQuery(Name = "quantity")] int quantity)
        {
            List<Producto> producto; //= await _context.Producto.Skip((page - 1) * quantity).Take(quantity).ToListAsync();
            
            if (page != 0 && quantity != 0)
            {
                producto = await _context.Producto.Skip((page - 1) * quantity).Take(quantity).ToListAsync();
            }
            else
            {
                producto = await _context.Producto.ToListAsync();
            }
            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }
    }
}
