[官方文档](https://readthedocs.org/projects/automapper/)
>安装Nuget
>> AutoMapper(必须),
>> AutoMapper.Extensions.Microsoft.DependencyInjection(依赖注入需要)

>注入项目中
 `services.AddAutoMapper(typeof(Startup)); //单纯使用services.AddAutoMapper()也可以，不过过时了;`

> 创建实体
  ```
  public  class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Other { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Sex { get; set; }
    }
  ```
> 初始化
  ```
  public class AutoMapperConfigs : Profile 
    {//必须继承Profile
        //添加你的实体映射关系.
        public AutoMapperConfigs()
        {
            var map = CreateMap<User, UserDto>();
            map.ForMember(o => o.Name, opt => opt.NullSubstitute("姓名"));//默认值添加
            map.ForMember(o => o.Sex, opt => opt.MapFrom(t=>t.Other));//字段转换
        }
    }
  ```
>使用
 ```
 public class ValuesController : ControllerBase
    {
        private readonly IMapper _mapper;

        public ValuesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public Object Get()
        {
            var user = new User() { Id = 1, Name = "132" };
            return  _mapper.Map<User,UserDto>(user);//将user转成UserDto
        }
 ```
