using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MLManager.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly MLManagerContext _ctx;
        public DatasetService(MLManagerContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Dataset> CreateDataset(int userId,string datasetName)
        {
            var dataset = new Dataset
            {
                UserId = userId,
                DatasetName = datasetName
            };

            var datasetEntity = _ctx.Datasets.Add(dataset);
            await _ctx.SaveChangesAsync();
            datasetEntity.State = EntityState.Detached;

            return dataset;
        }

        public async Task<IEnumerable<Dataset>> GetDatasets(int userId,int offset = 0,int size = 25)
        {
            return await _ctx.Datasets.Where(x => x.UserId == userId).Skip(offset).Take(size).ToListAsync();
        }
        
    }
}