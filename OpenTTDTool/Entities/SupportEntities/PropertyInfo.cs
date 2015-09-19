using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities.SupportEntities
{
    public class PropertyInfoId
    {
        public int PropertyId { get; set; }
        public Features? EntityType { get; set; }

        public PropertyInfoId(int id, Features? entity)
        {
            PropertyId = id;
            EntityType = entity;
        }

        public override bool Equals(object obj)
        {
            var propId = obj as PropertyInfoId;
            if (propId == null) return false;
            return PropertyId == propId.PropertyId && EntityType == propId.EntityType;
        }

        public override int GetHashCode()
        {
            return PropertyId * ((int?)EntityType ?? 1);
        }
    }

    public class PropertyInfo
    {
        public const FieldSizes DefaultLength = FieldSizes.Byte;

        public FieldSizes Length { get; set; } = DefaultLength;
        public string PropertyName { get; set; }
    }
}
