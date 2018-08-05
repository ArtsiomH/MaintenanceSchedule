using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MaintenanceScheduleDataLayer.Interfaces;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.EFContext;
using System;
using System.Threading.Tasks;

namespace MaintenanceScheduleDataLayer.Repositories
{
    class TransformerTypeRepository : IRepository<TransformerType>
    {
        private MaintenanceScheduleContext context;

        public TransformerTypeRepository(MaintenanceScheduleContext context)
        {
            this.context = context;
        }

        public void Create(TransformerType t)
        {
            context.TransformerTypes.Add(t);
        }
                
        public TransformerType Read(int id)
        {
            return context.TransformerTypes.First(x => x.TransformerTypeId == id);
        }

        public void Update(TransformerType t)
        {
            TransformerType oldTypeTransformer = Read(t.TransformerTypeId);
            UpdateEntityReflection.Update(oldTypeTransformer, t);
            context.Entry(oldTypeTransformer).State = EntityState.Modified;
        }

        public void Delete(TransformerType t)
        {
            TransformerType typeTransformer = Read(t.TransformerTypeId);
            context.TransformerTypes.Remove(typeTransformer);
        }

        public IEnumerable<TransformerType> GetAll()
        {
            return context.TransformerTypes.ToList();
        }

        public Task<TransformerType> ReadAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
