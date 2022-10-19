using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Pact.Provider.FunctionalTests
{
    [Route("item-search")]
    [ApiController]
    public class ItemSearchController : ControllerBase
    {
        [HttpPost]
        [Route("items")]
        public IActionResult GetItems(ProductClassRequest request)
        {
            if (request?.CurrentFilter != null && request.RequesterUserId != null)
            {
                return Ok(
                    new ProductClassesResponse
                    {
                        Items = new List<ProductClass>
                        {
                            new ProductClass
                            {
                                Id = 100,
                                Properties = new List<Property>
                                {
                                    new Property
                                    {
                                        Id = 11,
                                        TypeId = 1,
                                        TypeName = "category",
                                        Caption = "canson paper"
                                    },
                                    new Property
                                    {
                                        Id = 16,
                                        TypeId = 2,
                                        TypeName = "brand",
                                        Caption = "Magnum"
                                    },
                                    new Property
                                    {
                                        Id = 21,
                                        TypeId = 3,
                                        TypeName = "weight",
                                        Caption = "80 gr"
                                    }
                                },
                                Description = "Some description about the unique product"
                            },
                            new ProductClass
                            {
                                Id = 134,
                                Properties = new List<Property>
                                {
                                    new Property
                                    {
                                        Id = 11,
                                        TypeId = 1,
                                        TypeName = "category",
                                        Caption = "canson paper"
                                    },
                                    new Property
                                    {
                                        Id = 16,
                                        TypeId = 2,
                                        TypeName = "brand",
                                        Caption = "Magnum"
                                    },
                                    new Property
                                    {
                                        Id = 23,
                                        TypeId = 3,
                                        TypeName = "weight",
                                        Caption = "110 gr"
                                    }
                                },
                                Description = "Some description about the unique product"
                            }
                        }
                    }
                );
            }
            return BadRequest();
        }
    }
}