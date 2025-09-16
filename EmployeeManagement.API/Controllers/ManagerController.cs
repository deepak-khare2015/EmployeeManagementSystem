using EmployeeManagement.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.Entities;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ManagerController : Controller
    {
        //private readonly AppDbContext _appDbContext;
        //private readonly IGenericRepository<Manager> _ManagerRepo;// in case of generic repso
        //private readonly IManagerRepsository _ManagerRepo; //in case of custom repository

        //For Unit of Word
        private readonly IUnitOfWork _uow;

        //in case of generic repo
        //public ManagerController(IGenericRepository<Manager> managerRepo, AppDbContext appDbContext)
        //{
        //    _appDbContext = appDbContext;
        //    _ManagerRepo = managerRepo;
        //}

        //in case of custom repository
        //public ManagerController(IManagerRepsository managerRepo, AppDbContext appDbContext)
        //{
        //    _appDbContext = appDbContext;
        //    _ManagerRepo = managerRepo;
        //}

        public ManagerController(IUnitOfWork uow)
        {

            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetAll()
        {
            //var manager = await _ManagerRepo.GetAllAsync();

            var manager = await _uow.Managers.GetAllAsync();
            return Ok(manager);

        }
    }
}
