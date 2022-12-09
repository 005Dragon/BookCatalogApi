﻿using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Api.Contracts.RequestParameters;

public class CreateUserApiParameters
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }
}