using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using app.DAL.Models;
using AutoMapper;

namespace app.BLL
{
    public class MappingProgfile : Profile
    {
        public MappingProgfile()
        {
            CreateMap<UserDTO, User>();
            
        }
    }
}
