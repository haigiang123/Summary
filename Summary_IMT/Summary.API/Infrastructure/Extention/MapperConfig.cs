using AutoMapper;
using AutoMapper.Configuration;
using Summary.Model.Models;
using Summary.Share.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Summary.API.Infrastructure.Extention
{
    public static class MapperConfig
    {
        public static MapperConfiguration Config()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostCategory, PostCategoryVM>()
                   .ForMember(x => x.FullName, y => y.MapFrom(z => z.Name + z.ID));
                cfg.CreateMap<Post, PostVM>();
            });

            return config;
        }
    }
}