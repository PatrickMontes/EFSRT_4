using EFSRTproyecto.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFSRTproyecto.Controllers
{
    public class InventarioController : Controller
    {

        /**METODO PARA LISTAR CATEGORIAS**/

        public IEnumerable<Categoria> ListadoDeCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_listar_categoria", cn); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    categorias.Add(new Categoria()
                    {
                        IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                        NombreCategoria = dr.GetString(dr.GetOrdinal("NombreCategoria"))
                    });
                }

                dr.Close();
            }

            return categorias;
        }

        public ActionResult ListaCategorias()
        {
            return View(ListadoDeCategorias());
        }

        public string InsertarCategoria(Categoria reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertar_categoria", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombreCategoria", reg.NombreCategoria);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();

                        mensaje = $"Se ha insertado {i} categoría";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al insertar categoría: {ex.Message}";
                }
            }

            return mensaje;
        }

        public ActionResult CrearCategoria()
        {
            return View(new Categoria());
        }

        [HttpPost]
        public ActionResult CrearCategoria(Categoria reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }
            ViewBag.mensaje = InsertarCategoria(reg);
            return View(reg);
        }



        public string ActualizarCategoria(Categoria reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizar_categoria", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdCategoria", reg.IdCategoria);
                        cmd.Parameters.AddWithValue("@NombreCategoria", reg.NombreCategoria);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {i} registro";
                    }
                }
                catch (Exception ex) { mensaje = ex.Message; }
            }

            return mensaje;
        }

        public ActionResult EditCategoria(int id = 0)
        {
            Categoria reg = BuscarCategoria(id);

            if (reg == null)
            {
                return RedirectToAction("ListaCategorias");
            }

            return View(reg);
        }

        public Categoria BuscarCategoria(int id)
        {
            return ListadoDeCategorias().FirstOrDefault(m => m.IdCategoria == id);
        }

        [HttpPost]
        public ActionResult EditCategoria(Categoria reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }

            ViewBag.mensaje = ActualizarCategoria(reg);

            return View(reg);
        }

        public string EliminarCategoria(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_categoria", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCategoria", id);

                    cn.Open();

                    int i = cmd.ExecuteNonQuery();

                    mensaje = $"Se ha eliminado {i} categoría";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }

            return mensaje;
        }

        [HttpPost]
        public ActionResult EliminarCategorias(int id)
        {
            ViewBag.Mensaje = EliminarCategoria(id);
            return RedirectToAction("ListaCategorias");
        }


        /**METODO PARA LISTAR PROVEEDORES**/
        public IEnumerable<Proveedor> ListadoDeProveedor()
        {
            List<Proveedor> proveedores = new List<Proveedor>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_listar_proveedores", cn); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    proveedores.Add(new Proveedor()
                    {
                        IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                        NombreProveedor = dr.GetString(dr.GetOrdinal("NombreProveedor")),
                       direccion = dr.GetString(dr.GetOrdinal("direccion")),
                       Telefono = dr.GetString(dr.GetOrdinal("Telefono")),
                    });
                }

                dr.Close();
            }

            return proveedores;
        }

        public ActionResult ListaProvedores()
        {
            return View(ListadoDeProveedor());
        }

        public string InsertarProveedor(Proveedor reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertar_proveedor", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                        cmd.Parameters.AddWithValue("@Direccion", reg.direccion);
                        cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();

                        mensaje = $"Se ha insertado {i} proveedor";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al insertar proveedor: {ex.Message}";
                }
            }

            return mensaje;
        }

        public ActionResult CrearProveedor()
        {
            return View(new Proveedor());
        }

        [HttpPost]
        public ActionResult CrearProveedor(Proveedor reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }
            ViewBag.mensaje = InsertarProveedor(reg);
            return View(reg);
        }
        public string ActualizarProveedor(Proveedor reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizar_proveedor", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdProveedor", reg.IdProveedor);
                        cmd.Parameters.AddWithValue("@NombreProveedor", reg.NombreProveedor);
                        cmd.Parameters.AddWithValue("@Direccion", reg.direccion);
                        cmd.Parameters.AddWithValue("@Telefono", reg.Telefono);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {i} registro";
                    }
                }
                catch (Exception ex) { mensaje = ex.Message; }
            }

            return mensaje;
        }

        public ActionResult EditarProveedor(int id = 0)
        {
            Proveedor reg = BuscarProveedor(id);

            if (reg == null)
            {
                return RedirectToAction("ListaProvedores");
            }

            return View(reg);
        }

        public Proveedor BuscarProveedor(int id)
        {
            return ListadoDeProveedor().FirstOrDefault(m => m.IdProveedor == id);
        }

        [HttpPost]
        public ActionResult EditarProveedor(Proveedor reg)
        {
            if (!ModelState.IsValid)
            {
                return View(reg);
            }

            ViewBag.mensaje = ActualizarProveedor(reg);

            return View(reg);
        }

        public string EliminarProveedor(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarProveedor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProveedor", id);

                    cn.Open();

                    int i = cmd.ExecuteNonQuery();

                    mensaje = $"Se ha eliminado {i} proveedor";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }

            return mensaje;
        }

        [HttpPost]
        public ActionResult EliminarProveedores(int id)
        {
            ViewBag.Mensaje = EliminarProveedor(id);
            return RedirectToAction("ListaProvedores");
        }


        /**METODO PARA LISTAR PRODUCTOS**/
        public IEnumerable<Producto> ListadoDeProducto()
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_listar_productos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Add(new Producto()
                    {
                        id = dr.GetInt32(dr.GetOrdinal("IdProducto")),  
                        NombreProducto = dr.GetString(dr.GetOrdinal("NombreProducto")),
                        Descripcion = dr.GetString(dr.GetOrdinal("Descripcion")),
                        PrecioUnitario = dr.GetDecimal(dr.GetOrdinal("PrecioUnitario")),
                        Stock = dr.GetInt32(dr.GetOrdinal("Stock")),
                        categoria = new Categoria
                        {
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),  
                            NombreCategoria = dr.GetString(dr.GetOrdinal("NombreCategoria"))
                        },
                        proveedor = new Proveedor
                        {
                            IdProveedor = dr.GetInt32(dr.GetOrdinal("IdProveedor")),
                            NombreProveedor = dr.GetString(dr.GetOrdinal("NombreProveedor")),
                           
                        }
                    });
                }

                dr.Close();
            }

            return productos;

        }
       

        public ActionResult ListaProductos()
        {
            return View(ListadoDeProducto());
        }

        public string InsertarProducto(Producto reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_insertar_producto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NombreProducto", reg.NombreProducto);
                        cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", reg.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Stock", reg.Stock);
                        cmd.Parameters.AddWithValue("@IdCategoria", reg.categoria.IdCategoria);
                        cmd.Parameters.AddWithValue("@IdProveedor", reg.proveedor.IdProveedor);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();

                        mensaje = $"Se ha insertado {i} producto";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al insertar producto: {ex.Message}";
                }
            }

            return mensaje;
        }


        [HttpPost]
        public ActionResult CrearProducto(Producto reg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria", reg.categoria);
                ViewBag.proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor", reg.proveedor);
                return View(reg);
            }
            ViewBag.mensaje = InsertarProducto(reg);
            ViewBag.categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria",reg.categoria);
            ViewBag.proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor",reg.proveedor);
            return View(reg);
        }
        public ActionResult CrearProducto()
        {
            ViewBag.categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria");
            ViewBag.proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor");


            return View(new Producto());
        }

        public string ActualizarProducto(Producto reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_actualizar_producto", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                      
                        cmd.Parameters.AddWithValue("@IdProducto", reg.id);
                        cmd.Parameters.AddWithValue("@NombreProducto", reg.NombreProducto);
                        cmd.Parameters.AddWithValue("@Descripcion", reg.Descripcion);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", reg.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Stock", reg.Stock);
                        cmd.Parameters.AddWithValue("@IdCategoria", reg.categoria.IdCategoria);
                        cmd.Parameters.AddWithValue("@IdProveedor", reg.proveedor.IdProveedor);

                        cn.Open();

                        int i = cmd.ExecuteNonQuery();
                        mensaje = $"Se ha actualizado {i} registro";
                    }
                }
                catch (Exception ex) { mensaje = ex.Message; }
            }

            return mensaje;
        }
        public ActionResult EditarProducto(int id = 0)
        {
            Producto reg = BuscarProducto(id);

            if (reg == null)
            {
                return RedirectToAction("ListaProductos");
            }

            ViewBag.Categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria", reg.categoria.IdCategoria);
            ViewBag.Proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor", reg.proveedor.IdProveedor);

            return View(reg);
        }

        public Producto BuscarProducto(int id)
        {
            return ListadoDeProducto().FirstOrDefault(m => m.id == id);
        }

        [HttpPost]
        public ActionResult EditarProducto(Producto reg)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria", reg.categoria.IdCategoria);
                ViewBag.Proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor", reg.proveedor.IdProveedor);
                return View(reg);
            }

            ViewBag.mensaje = ActualizarProducto(reg);

            ViewBag.Categorias = new SelectList(ListadoDeCategorias(), "IdCategoria", "NombreCategoria", reg.categoria.IdCategoria);
            ViewBag.Proveedores = new SelectList(ListadoDeProveedor(), "IdProveedor", "NombreProveedor", reg.proveedor.IdProveedor);

            return View(reg);
        }


        public string EliminarProducto(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["sql"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_producto", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", id);

                    cn.Open();

                    int i = cmd.ExecuteNonQuery();

                    mensaje = $"Se ha eliminado {i} producto";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }

            return mensaje;
        }

        [HttpPost]
        public ActionResult EliminarProductos(int id)
        {
            ViewBag.Mensaje = EliminarProducto(id);
            return RedirectToAction("ListaProductos");
        }


    }
}