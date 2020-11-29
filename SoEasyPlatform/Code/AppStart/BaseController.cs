using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoEasyPlatform 
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected  IMapper mapper;
        protected Repository<Menu> MenuDb=> new Repository<Menu>();
        protected Repository<DBConnection> DBConnectionDb => new Repository<DBConnection>();
    }
}
