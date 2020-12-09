using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SoEasyPlatform.Code.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : BaseController
    {

        public TemplateController(IMapper mapper) : base(mapper)
        {

        }
    }
}
