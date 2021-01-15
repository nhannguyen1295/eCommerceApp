using System;
using System.Collections.Generic;
using System.Linq;
using eCommerceApp.Contract;
using eCommerceApp.Entities.DTO;
using eCommerceApp.Entities.LinkModels;
using eCommerceApp.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

namespace eCommerceApp.Server.Utility
{
    public class CategoryLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<CategoryDTO> _dataShaper;

        public CategoryLinks(LinkGenerator linkGenerator, IDataShaper<CategoryDTO> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        private List<Entity> ShapeData(IEnumerable<CategoryDTO> categoriesDTO, string fields)
        => _dataShaper.ShapeData(categoriesDTO, fields).Select(x => x.Entity).ToList();

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedCategories(List<Entity> shapedCategories)
        => new LinkResponse { ShapedEntities = shapedCategories };

        private LinkResponse ReturnLinkedCategories(IEnumerable<CategoryDTO> categoriesDTO, string fields, HttpContext httpContext, List<Entity> shapedCategories)
        {
            var categoriesDTOList = categoriesDTO.ToList();

            for (var index = 0; index < categoriesDTOList.Count(); index++)
            {
                var categoryLinks = CreateLinksForCategory(httpContext, categoriesDTOList[index].Id, fields);
                shapedCategories[index].Add("Links", categoryLinks);
            }

            var categoryCollection = new LinkCollectionWrapper<Entity>(shapedCategories);
            var linkedCategories = CreateLinksForCategories(httpContext, categoryCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedCategories };
        }

        private LinkCollectionWrapper<Entity> CreateLinksForCategories(HttpContext httpContext, LinkCollectionWrapper<Entity> categoryWrapper)
        {
            categoryWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetCategories", values: new { }),
                                               "self",
                                               "GET"));

            return categoryWrapper;
        }

        private List<Link> CreateLinksForCategory(HttpContext httpContext, Guid categoryId, string fields = "")
        {
            var links = new List<Link>{
                new Link(_linkGenerator.GetUriByAction(httpContext,"GetCategory",values:new{categoryId, fields}),
                         "self",
                         "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext,"DeleteCategory",values:new{categoryId, fields}),
                         "delete_category",
                         "DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext,"UpdateCategory",values:new{categoryId, fields}),
                         "update_category",
                         "PUT"),
                new Link(_linkGenerator.GetUriByAction(httpContext,"PartiallyUpdateCategory",values:new{categoryId, fields}),
                         "partially_update_category",
                         "PATCH")
            };

            return links;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<CategoryDTO> categoriesDTO,
                                             string fields,
                                             HttpContext httpContext)
        {
            var shapedCategories = ShapeData(categoriesDTO, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedCategories(categoriesDTO, fields, httpContext, shapedCategories);

            return ReturnShapedCategories(shapedCategories);
        }
    }
}