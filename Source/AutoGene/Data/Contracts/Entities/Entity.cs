using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Mobile.Server;

namespace Data.Contracts.Entities
{
    public class Entity: EntityData
    {
        public Entity()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}