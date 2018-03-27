using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreSPA.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreSPA.Web.Controllers
{
    [Route("api/Values"), Produces("application/json"), EnableCors("AppPolicy")]
    public class ValuesController : Controller
    {
        private dbCoreContext _ctx = null;
        public ValuesController(dbCoreContext context)
        {
            _ctx = context;
        }


        // GET: api/Values/GetUser
        [HttpGet, Route("GetUser")]
        public async Task<object> GetUser()
        {
            List<User> users = null;
            object result = null;
            try
            {
                using (_ctx)
                {
                    users = await _ctx.User.ToListAsync();
                    result = new
                    {
                        User
                    };
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return users;
        }

        // GET api/Values/GetUserByID/5
        [HttpGet, Route("GetUserByID/{id}")]
        public async Task<User> GetUserByID(int id)
        {
            User user = null;
            try
            {
                using (_ctx)
                {
                    user = await _ctx.User.FirstOrDefaultAsync(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return user;
        }


        // POST api/Values/PostUser
        [HttpPost, Route("PostUser")]
        public async Task<object> PostUser([FromBody]User model)
        {
            object result = null; string message = "";
            if (model == null)
            {
                return BadRequest();
            }
            using (_ctx)
            {
                using (var _ctxTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        _ctx.User.Add(model);
                        await _ctx.SaveChangesAsync();
                        _ctxTransaction.Commit();
                        message = "Saved Successfully";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback();
                        e.ToString();
                        message = "Saved Error";
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }

        // PUT api/Values/PutUser/5
        [HttpPut, Route("PutUser/{id}")]
        public async Task<object> PutUser(int id, [FromBody]User model)
        {
            object result = null; string message = "";
            if (model == null)
            {
                return BadRequest();
            }
            using (_ctx)
            {
                using (var _ctxTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var entityUpdate = _ctx.User.FirstOrDefault(x => x.Id == id);
                        if (entityUpdate != null)
                        {
                            entityUpdate.Name = model.Name;
                            entityUpdate.Phone = model.Phone;
                            entityUpdate.Email = model.Email;

                            await _ctx.SaveChangesAsync();
                        }
                        _ctxTransaction.Commit();
                        message = "Entry Updated";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback(); e.ToString();
                        message = "Entry Update Failed!!";
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }

        // DELETE api/Values/DeleteUserByID/5
        [HttpDelete, Route("DeleteUserByID/{id}")]
        public async Task<object> DeleteUserByID(int id)
        {
            object result = null; string message = "";
            using (_ctx)
            {
                using (var _ctxTransaction = _ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var idToRemove = _ctx.User.SingleOrDefault(x => x.Id == id);
                        if (idToRemove != null)
                        {
                            _ctx.User.Remove(idToRemove);
                            await _ctx.SaveChangesAsync();
                        }
                        _ctxTransaction.Commit();
                        message = "Deleted Successfully";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback(); e.ToString();
                        message = "Error on Deleting!!";
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }
    }
}
