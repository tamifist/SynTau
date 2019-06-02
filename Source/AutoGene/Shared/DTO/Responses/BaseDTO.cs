using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Shared.DTO.Converters;

namespace Shared.DTO.Responses
{
    public abstract class BaseDTO
    {
        public string Id { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public string Version { get; set; }
    }
}