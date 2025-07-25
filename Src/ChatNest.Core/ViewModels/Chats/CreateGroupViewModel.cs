﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ChatNest.Core.ViewModels.Chats
{
    public class CreateGroupViewModel
    {
        public long UserId { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        public string GroupName { get; set; }
    }
}