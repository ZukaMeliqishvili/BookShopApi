﻿using BookShopApi.Dto._Book;
using BookShopApi.Dto.Order;
using BookShopApi.Entities;
using Mapster;

namespace BookShopApi.Infrastructure.Extensions
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Book, BookGetDto>
                .NewConfig()
                .Map(dest => dest.Categories, src => src.Categories.
                Select(x => new { x.Category.Id, x.Category.Name }).ToList());
            //TypeAdapterConfig<Order, OrderResponseDto>
            //    .NewConfig()
            //    .Map(dest => dest.Book, src => src.Book);
            //TypeAdapterConfig.GlobalSettings.Default.Settings.MaxDepth = 2;
        }
    }
}
