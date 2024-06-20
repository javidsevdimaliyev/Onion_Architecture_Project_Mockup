﻿using SolutionName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionName.Application.Features.Queries.Products.GetAll
{
    public class GetProductQueryResponse
    {
        public ICollection<ProductEntity> Products { get; set; }
    }
}
