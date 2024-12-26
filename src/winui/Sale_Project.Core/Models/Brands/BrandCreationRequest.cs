﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sale_Project.Core.Models.Brands;

public class BrandCreationRequest
{
    [JsonPropertyName("code")]
    public int Id
    {
        get; set;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }
}
