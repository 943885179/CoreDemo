using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo_AutoMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IMapper _mapper;

        public ValuesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public UserDto Get()
        {
            // Mapper.Initialize(opt => opt.CreateMap<User, UserDto>());
            var user = new User() { Id = 1 ,Other="213",Other1="2019",Other2=DateTime.Now};
            var list = new List<User> {user,user };
            //var xx = list.Where(o => o.Id == 1);//ProjectTo<UserDto>().ToList()
            var dto = _mapper.Map<User, UserDto>(user);
            return dto;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class AutoMapperConfigs : Profile
    {
        //添加你的实体映射关系.
        public AutoMapperConfigs()
        {
            var map = CreateMap<User, UserDto>();
            map.ForMember(o => o.Name, opt => opt.NullSubstitute("姓名"));//默认值添加
           // map.ForMember(o => o.Name, opt => opt.Ignore());//排除该字段
            map.ForMember(o => o.Sex, opt => opt.MapFrom(t=>t.Other));//字段转换
            //嵌套查询
            CreateMap<OuterSource, OuterDest>();
            CreateMap<InnerSource, InnerDest>();
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Other { get; set; }
        public string Other1 { get; set; }
        public DateTime Other2 { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Other1 { get; set; }
        public string Other2 { get; set; }
    }

    public class OuterSource
    {
        public int Value { get; set; }
        public InnerSource Inner { get; set; }
    }

    public class InnerSource
    {
        public int OtherValue { get; set; }
    }
    public class OuterDest
    {
        public int Value { get; set; }
        public InnerDest Inner { get; set; }
    }

    public class InnerDest
    {
        public int OtherValue { get; set; }
    }
}
