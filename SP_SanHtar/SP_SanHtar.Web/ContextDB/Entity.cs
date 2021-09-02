using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP_SanHtar.Web.ContextDB
{
    [Serializable]
    public abstract class Entity
    {
        int? _requestedHashCode;
        Guid _id;

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }
        public bool Enabled { get; set; }


        public bool IsTransient()
        {
            return this._id == Guid.Empty;
        }

        public object Clone()
        {
            return this.MemberwiseClone();

        }
        /// <summary>
        /// Generate identity for this entity
        /// </summary>
        public Guid GenerateNewIdentity()
        {
            if (IsTransient())
                this._id = IdentityGenerator.NewSequentialGuid();

            return this._id;
        }
    }
}
